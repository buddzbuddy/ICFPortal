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
                    return Json(new { result = true, items = prevReport.ReportItems.Select(x => new { x.BalanceAmount, x.BalanceSum, x.HumDistributionPlanItemId }).ToList() }, JsonRequestBehavior.AllowGet);
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
