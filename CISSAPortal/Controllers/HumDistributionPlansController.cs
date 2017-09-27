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
using Intersoft.CISSA.DataAccessLayer.Model.Workflow;
using Intersoft.CISSA.DataAccessLayer.Model.Query.Builders;
using Intersoft.CISSA.DataAccessLayer.Model.Query;
using Intersoft.CISSA.DataAccessLayer.Core;
using Intersoft.CISSA.DataAccessLayer.Model.Context;
using Intersoft.CISSA.BizService.Utils;
using System.Data.OleDb;
using LinqToExcel;
using System.Data.Entity.Validation;
using System.Xml;
using System.Configuration;
using CISSAPortal.Models;

namespace CISSAPortal.Controllers
{
    [Authorize]
    public class HumDistributionPlansController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HumDistributionPlans
        public ActionResult Index(string messages = "")
        {
            if(messages != "")
            {
                ViewBag.Messages = messages.Split(',');
            }
            var humDistributionPlans = db.HumDistributionPlans.Include(h => h.Company).Where(x => x.Company.AspNetUser.UserName == User.Identity.Name);
            return View(humDistributionPlans.ToList());
        }

        public FileResult DownloadExcel()
        {

            string path = Server.MapPath("~/Doc/Plan.xlsx");//"/Doc/Plan.xlsx";
            return File(path, "application/vnd.ms-excel", "Plan.xlsx");
        }
        
        public ActionResult Create()
        {

            var uManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var userInfo = uManager.FindByNameAsync(User.Identity.Name).GetAwaiter().GetResult();
            var model = new HumDistributionPlan();
            if (userInfo.Companies.Count > 0)
            {
                model.CompanyId = userInfo.Companies.First().Id;
                model.Company = userInfo.Companies.First();

                var xmldoc = new XmlDocument();
                string urlOrigin = "http://www.nbkr.kg/XML/CurrenciesReferenceList.xml";
                string urlFake = Server.MapPath("~/Doc/CurrenciesReferenceList.xml");
                string url = urlFake;
                if (ConfigurationManager.AppSettings.AllKeys.Contains("HasInternetConnection") && ConfigurationManager.AppSettings["HasInternetConnection"] == "yes")
                    url = urlOrigin;

                xmldoc.Load(url);

                var xdoc = DocumentExtensions.ToXDocument(xmldoc);

                ViewBag.Currencies = from c in xdoc.Root.Elements()
                                     select c.Attribute("ISOCode").Value;
            }
            else
            {
                return RedirectToAction("Create", "Companies", new { userId = userInfo.Id, returnUrl = Request.Path });
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase FileUpload, [Bind(Include = "Id,CompanyId,Date,CurrencyISOCode")] HumDistributionPlan humDistributionPlan)
        {
            List<string> messages = new List<string>();
            var planItems = new List<HumDistributionPlanItem>();
            if (ModelState.IsValid)
            {
                if (FileUpload != null)
                {
                    if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        string filename = FileUpload.FileName;
                        string targetpath = Server.MapPath("~/TempDoc/");
                        FileUpload.SaveAs(targetpath + filename);
                        string pathToExcelFile = targetpath + filename;
                        var connectionString = "";
                        if (filename.EndsWith(".xls"))
                        {
                            connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                        }
                        else if (filename.EndsWith(".xlsx"))
                        {
                            connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                        }
                        string sheetName = "План распределения";
                        var adapter = new OleDbDataAdapter("SELECT * FROM [" + sheetName + "$]", connectionString);
                        var ds = new DataSet();

                        adapter.Fill(ds, "ExcelTable");

                        DataTable dtable = ds.Tables["ExcelTable"];



                        var excelFile = new ExcelQueryFactory(pathToExcelFile);
                        var humDistributionPlanItems = from a in excelFile.Worksheet<HumDistributionPlanItemModel>(sheetName) select a;
                        var units = db.UnitTypes.ToList();
                        
                        foreach (var item in humDistributionPlanItems)
                        {
                            try
                            {
                                var unitStr = item.Unit;

                                var unitType = units.FirstOrDefault(x => x.Name.ToLower() == unitStr.ToLower());
                                if (item.Consumer != "" &&
                                    item.Address != "" &&
                                    item.Region != "" &&
                                    item.ProductName != "" &&
                                    item.Unit != "" &&
                                    item.Amount != null &&
                                    item.Sum != null &&
                                    unitType != null)
                                {
                                    HumDistributionPlanItem objItem = new HumDistributionPlanItem();
                                    objItem.Consumer = item.Consumer;
                                    objItem.Address = item.Address;
                                    objItem.Region = item.Region;
                                    objItem.ProductName = item.ProductName;
                                    objItem.Amount = item.Amount;
                                    objItem.Sum = item.Sum;
                                    objItem.UnitTypeId = unitType.Id;

                                    planItems.Add(objItem);
                                }
                                else
                                {
                                    if (item.Consumer == "" || item.Consumer == null) messages.Add("Поле Consumer не может быть пустым");
                                    if (item.Region == "" || item.Region == null) messages.Add("Поле Region не может быть пустым");
                                    if (item.ProductName == "" || item.ProductName == null) messages.Add("Поле ProductName не может быть пустым");
                                    if (item.Amount == null) messages.Add("Поле Amount не может быть пустым");
                                    if (item.Sum == null) messages.Add("Поле Sum не может быть пустым");
                                    if (unitType == null) messages.Add("Поле Unit не может быть пустым, или оно не найдено в справочнике");
                                }
                            }

                            catch (DbEntityValidationException ex)
                            {
                                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                                {

                                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                                    {

                                        Response.Write("Свойство: " + validationError.PropertyName + " Текст ошибки: " + validationError.ErrorMessage);

                                    }

                                }
                            }
                        }
                        //deleting excel file from folder  
                        if ((System.IO.File.Exists(pathToExcelFile)))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                    }
                    else
                    {
                        messages.Add("Только Excel-формат доступен.");
                    }
                }

                if (messages.Count == 0)
                {
                    db.Database.BeginTransaction();
                    try
                    {
                        db.HumDistributionPlans.Add(humDistributionPlan);
                        db.SaveChanges();
                        foreach (var item in planItems)
                        {
                            item.HumDistributionPlanId = humDistributionPlan.Id;
                            db.HumDistributionPlanItems.Add(item);
                            db.SaveChanges();
                        }
                        db.Database.CurrentTransaction.Commit();
                        return RedirectToAction("Details", new { id = humDistributionPlan.Id });
                    }
                    catch(Exception e)
                    {
                        messages.Add("Error on saving items: " + e.Message);
                        db.Database.CurrentTransaction.Rollback();
                    }
                    finally
                    {
                        db.Database.Connection.Close();
                    }
                }
            }
            ViewBag.Messages = messages.ToArray();
            humDistributionPlan.Company = db.Companies.Find(humDistributionPlan.CompanyId);


            var xmldoc = new XmlDocument();
            string urlOrigin = "http://www.nbkr.kg/XML/CurrenciesReferenceList.xml";
            string urlFake = Server.MapPath("~/Doc/CurrenciesReferenceList.xml");
            string url = urlFake;
            if (ConfigurationManager.AppSettings.AllKeys.Contains("HasInternetConnection") && ConfigurationManager.AppSettings["HasInternetConnection"] == "yes")
                url = urlOrigin;

            xmldoc.Load(url);

            var xdoc = DocumentExtensions.ToXDocument(xmldoc);

            ViewBag.Currencies = from c in xdoc.Root.Elements()
                                 select c.Attribute("ISOCode").Value;

            return View(humDistributionPlan);
        }

        public ActionResult CreateWithRows()
        {

            var uManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var userInfo = uManager.FindByNameAsync(User.Identity.Name).GetAwaiter().GetResult();
            var model = new PlanViewModel { Items = new List<PlanItemViewModel> { new PlanItemViewModel() } };
            if (userInfo.Companies.Count > 0)
            {
                model.CompanyId = userInfo.Companies.First().Id;
                model.Company = userInfo.Companies.First();

                var xmldoc = new XmlDocument();
                string urlOrigin = "http://www.nbkr.kg/XML/CurrenciesReferenceList.xml";
                string urlFake = Server.MapPath("~/Doc/CurrenciesReferenceList.xml");
                string url = urlFake;
                if (ConfigurationManager.AppSettings.AllKeys.Contains("HasInternetConnection") && ConfigurationManager.AppSettings["HasInternetConnection"] == "yes")
                    url = urlOrigin;

                xmldoc.Load(url);

                var xdoc = DocumentExtensions.ToXDocument(xmldoc);

                ViewBag.Currencies = from c in xdoc.Root.Elements()
                                     select c.Attribute("ISOCode").Value;
                ViewBag.AreaId = new SelectList(db.Areas.ToList(), "Id", "Name");
                ViewBag.UnitTypeId = new SelectList(db.UnitTypes.ToList(), "Id", "Name");
            }
            else
            {
                return RedirectToAction("Create", "Companies", new { userId = userInfo.Id, returnUrl = Request.Path });
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateWithRows([Bind(Include = "CompanyId,Date,CurrencyISOCode,Items")] PlanViewModel plan)
        {
            if (plan.Items == null)
            {
                ModelState.AddModelError("", "Введите строки");
            }
            if (ModelState.IsValid)
            {
                var humPlan = new HumDistributionPlan
                {
                    CompanyId = plan.CompanyId,
                    CurrencyISOCode = plan.CurrencyISOCode,
                    Date = plan.Date
                };
                db.HumDistributionPlans.Add(humPlan);
                foreach(var planItem in plan.Items)
                {
                    var humPlanItem = new HumDistributionPlanItem
                    {
                        Address = planItem.Address,
                        Amount = planItem.Amount,
                        Consumer = planItem.Consumer,
                        HumDistributionPlanId = humPlan.Id,
                        ProductName = planItem.ProductName,
                        Sum = planItem.Sum,
                        UnitTypeId = planItem.UnitTypeId,
                        Region = db.Areas.Find(planItem.AreaId ?? 0).Name
                    };
                    db.HumDistributionPlanItems.Add(humPlanItem);
                }
                db.SaveChanges();
                return RedirectToAction("Details", new { id = humPlan.Id });
            }
            plan.Company = db.Companies.Find(plan.CompanyId);
            plan.Items = plan.Items == null ? new List<PlanItemViewModel>() : plan.Items;

            var xmldoc = new XmlDocument();
            string urlOrigin = "http://www.nbkr.kg/XML/CurrenciesReferenceList.xml";
            string urlFake = Server.MapPath("~/Doc/CurrenciesReferenceList.xml");
            string url = urlFake;
            if (ConfigurationManager.AppSettings.AllKeys.Contains("HasInternetConnection") && ConfigurationManager.AppSettings["HasInternetConnection"] == "yes")
                url = urlOrigin;

            xmldoc.Load(url);

            var xdoc = DocumentExtensions.ToXDocument(xmldoc);

            ViewBag.Currencies = from c in xdoc.Root.Elements()
                                 select c.Attribute("ISOCode").Value;

            ViewBag.AreaId = new SelectList(db.Areas.ToList(), "Id", "Name");
            ViewBag.UnitTypeId = new SelectList(db.UnitTypes.ToList(), "Id", "Name");

            return View(plan);
        }
        public ActionResult BlankEditorRow()
        {
            var model = new PlanItemViewModel();
            ViewBag.UnitTypeId = new SelectList(db.UnitTypes.ToList(), "Id", "Name");

            ViewBag.AreaId = new SelectList(db.Areas.ToList(), "Id", "Name");

            return PartialView(model);
        }
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HumDistributionPlan humDistributionPlan = db.HumDistributionPlans.Find(id);
            if (humDistributionPlan == null)
            {
                return HttpNotFound();
            }
            return View(humDistributionPlan);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HumDistributionPlan humDistributionPlan = db.HumDistributionPlans.Find(id);
            if (humDistributionPlan == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", humDistributionPlan.CompanyId);
            return View(humDistributionPlan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyId,Date")] HumDistributionPlan humDistributionPlan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(humDistributionPlan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", humDistributionPlan.CompanyId);
            return View(humDistributionPlan);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HumDistributionPlan humDistributionPlan = db.HumDistributionPlans.Find(id);
            if (humDistributionPlan == null)
            {
                return HttpNotFound();
            }
            return View(humDistributionPlan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HumDistributionPlan humDistributionPlan = db.HumDistributionPlans.Find(id);
            db.HumDistributionPlans.Remove(humDistributionPlan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult SetState(int reportId, int code)
        {
            var stateObj = db.DocumentStates.FirstOrDefault(x => x.Code == code);
            if (stateObj != null)
            {
                var reportObj = db.HumDistributionPlans.Find(reportId);
                if (reportObj != null)
                {
                    if (code == 1)
                    {
                        SendToCissa(reportObj);
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

        void SendToCissa(HumDistributionPlan obj)
        {
            var HumDistributionPlanDefId = new Guid("{8C9BC637-06E2-4AE7-ACD8-F498C1CA1620}");
            var HumDistributionPlanItemDefId = new Guid("{EAA7299C-E4AF-4203-8D18-1733F639F902}");
            var portalStateTypeId = new Guid("{D6D8589D-46EF-4323-B25F-BE312260F1BB}");
            var positionId = new Guid("{DF1C36BB-85B0-4C53-8729-F18A5D6615F4}");
            
            if (obj.Company != null)
            {
                var company = obj.Company;
                
                var cissameta = new CissaMeta.MetaProxy();
                var cissa_portal_users = cissameta.GetUsersByPositionId(positionId, company.OrgId ?? Guid.Empty);
                if (cissa_portal_users != null && cissa_portal_users.Count() > 0)
                {
                    var user = cissa_portal_users.First();
                    var context = CreateContext(user.User_Name, user.Id);
                    var docRepo = context.Documents;
                    var reportDoc = docRepo.New(HumDistributionPlanDefId);
                    reportDoc["RegDate"] = obj.Date;
                    reportDoc["LegalPerson"] = GetLegalPerson(context, obj);
                    reportDoc["PortalEntryId"] = obj.Id;
                    reportDoc["CurrencyISOCode"] = obj.CurrencyISOCode;
                    reportDoc["TotalSum"] = obj.HumDistributionPlanItems.Sum(x => x.Sum);
                    docRepo.Save(reportDoc);
                    docRepo.SetDocState(reportDoc, portalStateTypeId);

                    foreach (var objItem in obj.HumDistributionPlanItems)
                    {
                        var item = docRepo.New(HumDistributionPlanItemDefId);
                        item["HumDistributionPlan"] = reportDoc.Id;
                        item["Consumer"] = objItem.Consumer;
                        item["Region"] = objItem.Region;
                        item["Address"] = objItem.Address;
                        item["ProductName"] = objItem.ProductName;
                        item["Unit"] = objItem.UnitType.EnumId;
                        item["Amount"] = objItem.Amount;
                        item["Weight"] = objItem.Weight;
                        item["Sum"] = objItem.Sum;
                        docRepo.Save(item);
                    }

                }
                else
                    throw new ArgumentNullException(paramName: "cissa_portal_users", message: "Пользователь не найден!");
            }
            else
                throw new ArgumentNullException(paramName: "Report.Company", message: "Компания не найдена!");
        }

        Guid GetLegalPerson(WorkflowContext context, HumDistributionPlan obj)
        {
            var LegalPersonDefId = new Guid("{61AF4CEA-D77A-4305-8F0A-81A001BE5CFD}");

            var qb = new QueryBuilder(LegalPersonDefId, context.UserId);
            var query = context.CreateSqlQuery(qb.Def);

            query.AndCondition("PIN", ConditionOperation.Equal, obj.Company.INN);
            query.AndCondition("LegalName", ConditionOperation.Equal, obj.Company.Name);

            query.AddAttribute("&Id");
            using (var reader = context.CreateSqlReader(query))
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
