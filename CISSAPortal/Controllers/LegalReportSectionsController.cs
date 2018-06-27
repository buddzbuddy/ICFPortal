using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Intersoft.CISSA.DataAccessLayer.Core;
using Intersoft.CISSA.DataAccessLayer.Model.Context;
using Intersoft.CISSA.BizService.Utils;
using Intersoft.CISSA.DataAccessLayer.Model.Workflow;
using Intersoft.CISSA.DataAccessLayer.Model.Documents;
using Intersoft.CISSA.DataAccessLayer.Model.Query.Builders;
using Intersoft.CISSA.DataAccessLayer.Model.Query;

namespace CISSAPortal.Controllers
{
    public class LegalReportSectionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LegalReportSections
        public ActionResult Index()
        {
            var legalReportSections = db.LegalReportSections.Include(l => l.Company).Where(x => x.Company.AspNetUser.UserName == User.Identity.Name);
            return View(legalReportSections.ToList());
        }

        // GET: LegalReportSections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LegalReportSection legalReportSection = db.LegalReportSections.Find(id);
            if (legalReportSection == null)
            {
                return HttpNotFound();
            }
            return View(legalReportSection);
        }

        // GET: LegalReportSections/Create
        public async Task<ActionResult> Create()
        {
            var uManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var userInfo = await uManager.FindByNameAsync(User.Identity.Name);
            //ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name");
            var model = new LegalReportSection();
            if(userInfo.Companies.Count > 0)
            {
                model.CompanyId = userInfo.Companies.First().Id;
                model.Company = userInfo.Companies.First();
            }
            else
            {
                return RedirectToAction("Create", "Companies", new { userId = userInfo.Id, returnUrl = Request.Path });
            }
            return View(model);
        }

        // POST: LegalReportSections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CompanyId,ReportDate,BalanceBegin,AccruedBenefitsBegin,IncludingMonth,IncreaseDecreaseAmount,RefundedRepBudgetBegin,DebtBalanceEnd")] LegalReportSection legalReportSection)
        {
            if (legalReportSection.ReportDate <= new DateTime(2017, 1, 1))
                ModelState.AddModelError("ReportDate", "Ошибка в дате");

            if (ModelState.IsValid)
            {
                db.LegalReportSections.Add(legalReportSection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var uManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var userInfo = await uManager.FindByNameAsync(User.Identity.Name);
            legalReportSection.Company = userInfo.Companies.First();
            //ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", legalReportSection.CompanyId);
            return View(legalReportSection);
        }

        // GET: LegalReportSections/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LegalReportSection legalReportSection = db.LegalReportSections.Find(id);
            if (legalReportSection == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", legalReportSection.CompanyId);
            return View(legalReportSection);
        }

        // POST: LegalReportSections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyId,ReportDate,BalanceBegin,AccruedBenefitsBegin,IncludingMonth,IncreaseDecreaseAmount,RefundedRepBudgetBegin,DebtBalanceEnd")] LegalReportSection legalReportSection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(legalReportSection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", legalReportSection.CompanyId);
            return View(legalReportSection);
        }

        // GET: LegalReportSections/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LegalReportSection legalReportSection = db.LegalReportSections.Find(id);
            if (legalReportSection == null)
            {
                return HttpNotFound();
            }
            return View(legalReportSection);
        }

        // POST: LegalReportSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LegalReportSection legalReportSection = db.LegalReportSections.Find(id);
            db.LegalReportSections.Remove(legalReportSection);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SetState(int reportId, int code)
        {
            var stateObj = db.DocumentStates.FirstOrDefault(x => x.Code == code);
            if (stateObj != null)
            {
                var reportObj = db.LegalReportSections.Find(reportId);
                if (reportObj != null)
                {
                    SendToCissa(reportObj);
                    if (code == 1)
                    {
                        
                    }
                    reportObj.StateId = stateObj.Id;
                    db.Entry(reportObj).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = reportId });
                }
                else
                    return HttpNotFound("Отчет не найден! Id=" + reportId);
            }
            return HttpNotFound("Статус с кодом '" + code + "' не найден!");
        }

        void SendToCissa(LegalReportSection obj)
        {
            var LegalReportSectionDefId = new Guid("{EC14A952-D4D8-4EA8-90F5-50121CC4B173}");
            var BirthInfoOnPayBenefitDefId = new Guid("{46C27FED-B9E0-4DAA-9DD4-F984A526D9B5}");
            var DeadInfoOnPayBenefitDefId = new Guid("{89ADB43B-0D29-4287-AF5B-0E0DCED18B01}");
            var personDefId = new Guid("{D71CE61A-9B59-4B5E-8713-8131DBB5BA02}");
            var portalStateTypeId = new Guid("{D6D8589D-46EF-4323-B25F-BE312260F1BB}");
            var positionId = new Guid("{DF1C36BB-85B0-4C53-8729-F18A5D6615F4}");
            var maleEnumItemId = new Guid("{74C6C7FE-53C6-4492-A62F-65A7A49AB644}");
            var femaleEnumItemId = new Guid("{56E07640-5B5B-47FA-832D-A6639F36EB71}");


            if (obj.Company != null)
            {
                var company = obj.Company;

                //var RGUSOrgId = new Guid("{6853C82D-751E-40DD-AA14-21AF0AB7C64E}");
                var cissameta = new CissaMeta.MetaProxy();
                //var code = cissameta.OrgCode(company.OrgId ?? Guid.Empty);
                var cissa_portal_users = cissameta.GetUsersByPositionId(positionId, company.OrgId ?? Guid.Empty);
                if (cissa_portal_users != null && cissa_portal_users.Count() > 0)
                {
                    var user = cissa_portal_users.First();
                    var context = CreateContext(user.User_Name, user.Id);
                    var docRepo = context.Documents;
                    var reportDoc = docRepo.New(LegalReportSectionDefId);
                    reportDoc["RegDate"] = obj.ReportDate;
                    reportDoc["LegalPerson"] = GetLegalPerson(context, obj);
                    reportDoc["ReportDate"] = obj.ReportDate.Value.ToShortDateString();
                    reportDoc["BalanceBegin"] = obj.BalanceBegin;
                    reportDoc["AccruedBenefitsBegin"] = obj.AccruedBenefitsBegin;
                    reportDoc["IncludingMonth"] = obj.IncludingMonth;
                    reportDoc["IncreaseDecreaseAmount"] = obj.IncreaseDecreaseAmount;
                    reportDoc["RefundedRepBudgetBegin"] = obj.RefundedRepBudgetBegin;
                    reportDoc["DebtBalanceEnd"] = obj.DebtBalanceEnd;
                    docRepo.Save(reportDoc);
                    docRepo.SetDocState(reportDoc, portalStateTypeId);

                    foreach (var objItem in obj.BirthInfoOnPayBenefits)
                    {
                        var item = docRepo.New(BirthInfoOnPayBenefitDefId);
                        item["LegalReportSection"] = reportDoc.Id;
                        var person = docRepo.New(personDefId);
                        person["PIN"] = objItem.PIN;
                        person["LastName"] = objItem.LastName;
                        person["FirstName"] = objItem.FirstName;
                        person["MiddleName"] = objItem.MiddleName;
                        person["BirthDate"] = objItem.BirthDate;
                        docRepo.Save(person);
                        item["Person"] = person.Id;
                        item["Nationality"] = objItem.Citizenship;
                        item["BirthNumberCertificate"] = objItem.BirthCertificateNo;
                        item["DateFrom"] = objItem.DateFrom;
                        item["DateTo"] = objItem.DateTo;
                        item["SixDay"] = objItem.SixDay;
                        item["Highlands"] = objItem.Highlands;
                        item["SalaryOf3Months"] = objItem.SalaryOf3Months;
                        item["BirthCountWorkingDays"] = objItem.BirthCountWorkingDays;
                        item["AmountAverageEarnings"] = objItem.AmountAverageEarnings;
                        item["BirthCashThrowLegalOrg"] = objItem.BirthCashThrowLegalOrg;
                        item["BirthCadThrowRepublicBudget"] = objItem.BirthCadThrowRepublicBudget;
                        item["BirthTotalAmount"] = objItem.BirthTotalAmount;
                        docRepo.Save(item);
                    }

                    foreach (var objItem in obj.DeadInfoOnPayBenefits)
                    {
                        var item = docRepo.New(DeadInfoOnPayBenefitDefId);
                        item["LegalReportSection"] = reportDoc.Id;
                        var person = docRepo.New(personDefId);
                        person["PIN"] = objItem.PIN;
                        person["LastName"] = objItem.LastName;
                        person["FirstName"] = objItem.FirstName;
                        person["MiddleName"] = objItem.MiddleName;
                        person["BirthDate"] = objItem.BirthDate;
                        docRepo.Save(person);
                        item["Person"] = person.Id;
                        item["Citizenship"] = objItem.Citizenship;
                        item["DeadCadThrowRepublicBudget"] = objItem.DeadCadThrowRepublicBudget;
                        docRepo.Save(item);
                    }

                }
            }
            else
                throw new ArgumentNullException(paramName: "Report.Company", message: "Компания не найдена!");
        }

        Guid GetLegalPerson(WorkflowContext context, LegalReportSection obj)
        {
            var LegalPersonDefId = new Guid("{61AF4CEA-D77A-4305-8F0A-81A001BE5CFD}");

            var qb = new QueryBuilder(LegalPersonDefId, context.UserId);
            var query = context.CreateSqlQuery(qb.Def);

            query.AndCondition("PIN", ConditionOperation.Equal, obj.Company.INN);
            query.AndCondition("LegalName", ConditionOperation.Equal, obj.Company.Name);

            query.AddAttribute("&Id");
            using(var reader = context.CreateSqlReader(query))
            {
                if (reader.Read()) return reader.GetGuid(0);
            }
            var docRepo = context.Documents;
            var newLegalPerson = docRepo.New(LegalPersonDefId);

            newLegalPerson["PIN"] = obj.Company.INN;
            newLegalPerson["LegalName"] = obj.Company.Name;
            newLegalPerson["CodeOrg"] = obj.Company.OKPO;
            newLegalPerson["LegalAddress"] = obj.Company.Address;
            newLegalPerson["LegalPhone"] = obj.Company.Telephone;
            newLegalPerson["Email"] = obj.Company.Email;
            newLegalPerson["TypeOfActivity"] = obj.Company.ActivityType;
            newLegalPerson["BankName"] = obj.Company.BankName;
            newLegalPerson["BankIdCode"] = obj.Company.BIK;
            newLegalPerson["CheckAccount"] = obj.Company.BankAccountNo;

            docRepo.Save(newLegalPerson);

            return newLegalPerson.Id;
        }

        static IAppServiceProvider InitProvider(string username, Guid userId)
        {
            var dataContextFactory = DataContextFactoryProvider.GetFactory();

            var dataContext = dataContextFactory.CreateMultiDc("DataContexts");
            BaseServiceFactory.CreateBaseServiceFactories();
            var providerFactory = AppServiceProviderFactoryProvider.GetFactory();
            var provider = providerFactory.Create(dataContext);
            var serviceRegistrator = provider.Get<IAppServiceProviderRegistrator>();
            serviceRegistrator.AddService(new UserDataProvider(userId, username));
            return provider;
        }

        static WorkflowContext CreateContext(string username, Guid userId)
        {
            return new WorkflowContext(new WorkflowContextData(Guid.Empty, userId), InitProvider(username, userId));
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
