using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using Intersoft.CISSA.DataAccessLayer.Model.Workflow;
using Intersoft.CISSA.DataAccessLayer.Model.Query.Builders;
using Intersoft.CISSA.DataAccessLayer.Core;
using Intersoft.CISSA.DataAccessLayer.Model.Context;
using Intersoft.CISSA.BizService.Utils;

namespace CISSAPortal.Controllers
{
    public class DeadInfoOnPayBenefitsController : Controller
    {

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
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DeadInfoOnPayBenefits
        public ActionResult Index()
        {
            var deadInfoOnPayBenefits = db.DeadInfoOnPayBenefits.Include(d => d.Gender).Include(d => d.Report);
            return View(deadInfoOnPayBenefits.ToList());
        }

        // GET: DeadInfoOnPayBenefits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeadInfoOnPayBenefit deadInfoOnPayBenefit = db.DeadInfoOnPayBenefits.Find(id);
            if (deadInfoOnPayBenefit == null)
            {
                return HttpNotFound();
            }
            return View(deadInfoOnPayBenefit);
        }

        // GET: DeadInfoOnPayBenefits/Create
        public ActionResult Create(int reportId)
        {
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name");
            //ViewBag.ReportId = new SelectList(db.LegalReportSections, "Id", "Id");
            return View(new DeadInfoOnPayBenefit { ReportId = reportId });
        }

        // POST: DeadInfoOnPayBenefits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ReportId,PIN,LastName,FirstName,MiddleName,BirthDate,GenderId,DeadCertificateNo,DeadDateOfCertificate,Citizenship,DeadCadThrowRepublicBudget")] DeadInfoOnPayBenefit deadInfoOnPayBenefit)
        {
            if (ModelState.IsValid)
            {
                var positionId = new Guid("{DF1C36BB-85B0-4C53-8729-F18A5D6615F4}");

                var report = db.LegalReportSections.Find(deadInfoOnPayBenefit.ReportId);
                var company = db.Companies.Find(report.CompanyId);
                if (company != null)
                {
                    //var RGUSOrgId = new Guid("{6853C82D-751E-40DD-AA14-21AF0AB7C64E}");
                    var cissameta = new CissaMeta.MetaProxy();
                    //var code = cissameta.OrgCode(company.OrgId ?? Guid.Empty);
                    var cissa_portal_users = cissameta.GetUsersByPositionId(positionId, company.OrgId ?? Guid.Empty);
                    if (cissa_portal_users != null && cissa_portal_users.Count() > 0)
                    {
                        var user = cissa_portal_users.First();
                        var context = CreateContext(user.User_Name, user.Id);
                        try
                        {
                            CalcDeadInfoOnPayBenefits.Execute(context, deadInfoOnPayBenefit);

                            db.DeadInfoOnPayBenefits.Add(deadInfoOnPayBenefit);
                            db.SaveChanges();
                            return RedirectToAction("Details", "LegalReportSections", new { id = deadInfoOnPayBenefit.ReportId });
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError("", e.Message);
                        }
                    }
                }
            }

            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name", deadInfoOnPayBenefit.GenderId);
            return View(deadInfoOnPayBenefit);
        }

        // GET: DeadInfoOnPayBenefits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeadInfoOnPayBenefit deadInfoOnPayBenefit = db.DeadInfoOnPayBenefits.Find(id);
            if (deadInfoOnPayBenefit == null)
            {
                return HttpNotFound();
            }
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name", deadInfoOnPayBenefit.GenderId);
            ViewBag.ReportId = new SelectList(db.LegalReportSections, "Id", "Id", deadInfoOnPayBenefit.ReportId);
            return View(deadInfoOnPayBenefit);
        }

        // POST: DeadInfoOnPayBenefits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ReportId,PIN,LastName,FirstName,MiddleName,BirthDate,GenderId,DeadCertificateNo,DeadDateOfCertificate,Citizenship,DeadCadThrowRepublicBudget")] DeadInfoOnPayBenefit deadInfoOnPayBenefit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deadInfoOnPayBenefit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name", deadInfoOnPayBenefit.GenderId);
            ViewBag.ReportId = new SelectList(db.LegalReportSections, "Id", "Id", deadInfoOnPayBenefit.ReportId);
            return View(deadInfoOnPayBenefit);
        }

        // GET: DeadInfoOnPayBenefits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeadInfoOnPayBenefit deadInfoOnPayBenefit = db.DeadInfoOnPayBenefits.Find(id);
            if (deadInfoOnPayBenefit == null)
            {
                return HttpNotFound();
            }
            return View(deadInfoOnPayBenefit);
        }

        // POST: DeadInfoOnPayBenefits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeadInfoOnPayBenefit deadInfoOnPayBenefit = db.DeadInfoOnPayBenefits.Find(id);
            db.DeadInfoOnPayBenefits.Remove(deadInfoOnPayBenefit);
            db.SaveChanges();
            return RedirectToAction("Index");
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

    public static class CalcDeadInfoOnPayBenefits
    {
        public static void Execute(WorkflowContext context, DeadInfoOnPayBenefit deadInfoOnPayBenefit)
        {
            if ((DateTime)deadInfoOnPayBenefit.DeadDateOfCertificate.Value.AddMonths(6) <= DateTime.Now)
            {
                throw new ApplicationException("С даты смерти гражданина прошло 6 месяцев!");
                /*context.AddErrorMessage("DateOfIssue", "С даты смерти гражданина прошло 6 месяцев!");
                context.SuccessFlag = false;*/
            }

            //для юр. лица 100% выплата
            var amount = GetAmount(context);
            deadInfoOnPayBenefit.DeadCadThrowRepublicBudget = Math.Round(amount);
        }
        //Размер пособия по беременности/родом и для ритуального погребения
        private static readonly Guid AmountRitualBenefitDefId = new Guid("{07031123-E5DA-4D8B-AE85-446F9981E5A5}");
        private static readonly Guid TypePaysEnumId = new Guid("{0B945292-FD50-4FBE-8855-79B61A094DAE}"); //Вид выплаты спр.
        private static decimal GetAmount(WorkflowContext context)
        {
            var today = DateTime.Today;
            var aqb = new QueryBuilder(AmountRitualBenefitDefId);
            aqb.Where("From").Le(today).And("To").Gt(today).And("TypePays").Eq(TypePaysEnumId);
            using (var query = context.CreateSqlQuery(aqb.Def))
            {
                query.AddAttribute("Amount");
                query.AddOrderAttribute("From", false);
                using (var reader = context.CreateSqlReader(query))
                {
                    if (!reader.Read())
                        throw new ApplicationException("Размер пособия в справочнике не указан!");
                    return reader.Reader.GetDecimal(0);
                }
            }
        }
    }
}
