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
using Intersoft.Cissa.Report.Xls;
using Intersoft.Cissa.Report.Styles;
using System.Drawing;
using NPOI.SS.UserModel;
using System.IO;

namespace CISSAPortal.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Reports
        public async Task<ActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View((db.Reports.Include(r => r.User) as IEnumerable<Report> ?? new List<Report>()).ToList());
            }
            else
            {
                var currentUser = await UserManager.FindByNameAsync(User.Identity.Name);
                return View(db.Reports != null ? db.Reports.Where(x => x.UserId == currentUser.Id).ToList() : new List<Report>());
            }
        }
        [AllowAnonymous]
        public ActionResult ReportListPartial(int planId)
        {
            ViewBag.PlanId = planId;
            return PartialView(db.Reports != null ? db.Reports.Where(x => x.HumDistributionPlanId == planId).ToList() : new List<Report>());
        }

        // GET: Reports/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            foreach(var item in report.ReportItems)
            {
                item.HumDistributionPlanItem = db.HumDistributionPlanItems.Find(item.HumDistributionPlanItemId);
            }

            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }
        public FileResult GetFile(int reportId)
        {
            var report = db.Reports.Find(reportId);
            var HumDistributionPlan = db.HumDistributionPlans.Find(report.HumDistributionPlanId ?? 0);

            string filepath = "C://CissaFiles//Report.xls";//Server.MapPath("~/Doc/ExcelPlan.xls");
            using (var def = new XlsDef())
            {
                def.AddArea().AddRow().AddEmptyCell();
                var s5 = def.AddArea().AddRow();
                s5.AddText("Отчет получателя \"" + HumDistributionPlan.Company.Name + "\" о предоставлении гуманитарной помощи", 10);
                s5.Style.HAlign = HAlignment.Center; //По центру
                s5.Style.Bold();
                def.AddArea().AddEmptyRow();
                var h = def.AddArea().AddRow();
                h.AddText("№");
                h.AddText("Потребитель / Организация");
                h.AddText("Регион");
                h.AddText("Адрес");
                h.AddText("Наименование гум. помощи (товара)");
                h.AddText("Ед. изм.");
                h.AddText("Кол-во (план)");
                //h.AddText("Вес (кг) (план)");
                h.AddText("Сумма (сом)(план)*");
                h.AddText("Кол-во (факт)");
                //h.AddText("Вес (кг) (факт)");
                h.AddText("Сумма (факт)*");
                h.AddText("Кол-во (остаток)");
                //h.AddText("Вес (кг) (остаток)");
                h.AddText("Сумма (остаток)*");
                h.ShowAllBorders(true);
                h.Style.FontStyle = FontStyle.Bold; //Шрифт жирный
                h.Style.HAlign = HAlignment.Center; //По центру
                h.Style.BgColor = IndexedColors.BLUE_GREY.Index; //48; Цвет шапки
                h.Style.FontColor = IndexedColors.WHITE.Index; //Цвет шрифта
                h.Style.WrapText = true;
                h.Style.AutoWidth = true;
                h.Style.AutoHeight = true;
                int i = 1;
                double planAmount = 0;
                decimal planSum = 0;
                double factAmount = 0;
                decimal factSum = 0;
                double balanceAmount = 0;
                decimal balanceSum = 0;
                foreach (var item in db.ReportItems.Where(x => x.ReportId == reportId).Include(x => x.HumDistributionPlanItem).ToList())
                {
                    var r = def.AddArea().AddRow();
                    r.AddColumn().AddInt(i);
                    r.AddColumn().AddText(item.HumDistributionPlanItem.Consumer.Name);
                    r.AddColumn().AddText(item.HumDistributionPlanItem.Area.Name);
                    r.AddColumn().AddText(item.HumDistributionPlanItem.Address);
                    r.AddColumn().AddText(item.HumDistributionPlanItem.Product.Name);
                    r.AddColumn().AddText(item.HumDistributionPlanItem.UnitType.Name);
                    r.AddColumn().AddFloat(item.HumDistributionPlanItem.Amount ?? 0);
                    //r.AddColumn().AddText("");
                    r.AddColumn().AddFloat((double)(item.HumDistributionPlanItem.Sum ?? 0));
                    r.AddColumn().AddFloat(item.FactAmount ?? 0);
                    //r.AddColumn().AddText("");
                    r.AddColumn().AddFloat((double)(item.FactSum ?? 0));
                    r.AddColumn().AddFloat(item.BalanceAmount ?? 0);
                    //r.AddColumn().AddText("");
                    r.AddColumn().AddFloat((double)(item.BalanceSum ?? 0));
                    r.ShowAllBorders(true);
                    i++;

                    planAmount += item.HumDistributionPlanItem.Amount ?? 0;
                    planSum += item.HumDistributionPlanItem.Sum ?? 0;
                    factAmount += item.FactAmount ?? 0;
                    factSum += item.FactSum ?? 0;
                    balanceAmount += item.BalanceAmount ?? 0;
                    balanceSum += item.BalanceSum ?? 0;
                }
                var f = def.AddArea().AddRow();
                f.AddText("Итого:", 6);
                f.AddFloat(planAmount);
                f.AddFloat((double)planSum);
                f.AddFloat(factAmount);
                f.AddFloat((double)factSum);
                f.AddFloat(balanceAmount);
                f.AddFloat((double)balanceSum);
                f.ShowAllBorders(true);
                f.Style.HAlign = HAlignment.Right;
                f.Style.Bold();

                def.AddArea().AddRow().AddEmptyCell();
                var s1 = def.AddArea().AddRow();
                s1.AddText("ФИО руководителя организации", 3);
                var s3 = def.AddArea().AddRow();
                s3.AddText("______________________", 3);
                s3.AddText("подпись");
                def.AddArea().AddEmptyRow();
                var s4 = def.AddArea().AddRow();
                s4.AddText("Дата: ", 3);
                s4.AddText("«    » _____________ 20___");
                def.AddArea().AddEmptyRow();
                var s4_1 = def.AddArea().AddRow();
                s4_1.AddText("М.П.", 3);
                def.AddArea().AddEmptyRow();
                var s4_2 = def.AddArea().AddRow();
                s4_2.AddText("* сумма указана донором только для таможенных целей", 3);
                var builder = new XlsBuilder(def);
                var workbook = builder.Build();


                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    workbook.Write(stream);
                }
                return File(filepath, "application/vnd.ms-excel", "Report.xls");
            }
        }
        // GET: Reports/Create
        public ActionResult Create(int? humDistributionPlanId)
        {
            if (humDistributionPlanId == null)
                return RedirectToAction("SelectHumDistributionPlan");

            var user = UserManager.FindByNameAsync(User.Identity.Name).GetAwaiter().GetResult();
            var report = new Report { UserId = user.Id, User = user, HumDistributionPlanId = humDistributionPlanId };

            //Init items from plan
            var planItems = db.HumDistributionPlanItems.Where(x => x.HumDistributionPlanId == humDistributionPlanId).ToList();
            var unitTypes = db.UnitTypes.ToList();
            report.ReportItems = new List<ReportItem>();
            foreach (var planItem in planItems)
            {
                var reportItem = new ReportItem
                {
                    HumDistributionPlanItemId = planItem.Id,
                    HumDistributionPlanItem = planItem
                };
                report.ReportItems.Add(reportItem);
            }

            return View(report);
        }

        public ActionResult SelectHumDistributionPlan()
        {
            var humDistributionPlans = db.HumDistributionPlans.Include(h => h.Company).Where(x => x.Company.AspNetUser.UserName == User.Identity.Name);
            return View(humDistributionPlans.ToList());
        }

        public ActionResult SelectPlan(int planId)
        {
            return RedirectToAction("Create", new { humDistributionPlanId = planId });
        }

        // POST: Reports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Year,Quarter,Date,UserId,HumDistributionPlanId")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Reports.Add(report);
                db.SaveChanges();
                
                return RedirectToAction("Details", new { id = report.Id });
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", report.UserId);
            return View(report);
        }

        [HttpPost]
        public ActionResult CreateReport(Report report)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Reports.Add(report);
                    db.SaveChanges();

                    return Json(new { result = "success", report = report }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = "error", message = ModelState.Where(x => x.Value.Errors.Count > 0).First().Value.Errors.First().ErrorMessage }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(new { result = "error", message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CreateItems(List<ReportItem> items)
        {
            try
            {
                if(items != null)
                foreach(var item in items)
                {
                    if(item.HumDistributionPlanItemId != null)
                    {
                        db.ReportItems.Add(item);
                        db.SaveChanges();
                    }
                }
                
                return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = "error", message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetPrevReportItems(int planId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userInfo = UserManager.Users.First(x => x.UserName == User.Identity.Name);
                var reportsByPlanId = db.Reports.Where(x => x.UserId == userInfo.Id && x.HumDistributionPlanId == planId).ToList();
                if (reportsByPlanId.Count > 0)
                {
                    var prevReport = reportsByPlanId.First(x => x.Id == reportsByPlanId.Max(i => i.Id));
                    
                    return Json(new { result = true, items = prevReport.ReportItems.Select(x => new { x.BalanceAmount, x.BalanceSum, x.HumDistributionPlanItemId }) }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { result = true, items = new List<ReportItem>() }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { result = false, errorMessage = "Пользователь не авторизован!" }, JsonRequestBehavior.AllowGet);
        }

        // GET: Reports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", report.UserId);
            return View(report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Year,Month,Date,UserId")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = report.Id });
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", report.UserId);
            return View(report);
        }

        // GET: Reports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Report report = db.Reports.Find(id);
            db.Reports.Remove(report);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SetState(int reportId, int code)
        {
            var stateObj = db.DocumentStates.FirstOrDefault(x => x.Code == code);
            if(stateObj != null)
            {
                var reportObj = db.Reports.Find(reportId);
                if(reportObj != null)
                {
                    reportObj.StateId = stateObj.Id;
                    db.Entry<Report>(reportObj).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = reportId });
                }
                else
                    return HttpNotFound("Report not found! Id=" + reportId);
            }
            return HttpNotFound("State of '" + stateObj.Name + "' not found!");
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
