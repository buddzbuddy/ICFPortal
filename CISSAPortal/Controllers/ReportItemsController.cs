using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;

namespace CISSAPortal.Controllers
{
    public class ReportItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ReportItems
        public ActionResult Index()
        {
            var reportItems = db.ReportItems.Include(r => r.Report);
            return View(reportItems.ToList());
        }

        // GET: ReportItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportItem reportItem = db.ReportItems.Find(id);
            if (reportItem == null)
            {
                return HttpNotFound();
            }
            return View(reportItem);
        }

        // GET: ReportItems/Create
        public ActionResult Create(int reportId)
        {
            //ViewBag.ReportId = new SelectList(db.Reports, "Id", "UserId");
            ViewBag.UnitTypeId = new SelectList(db.UnitTypes, "Id", "Name");
            return View(new ReportItem { ReportId = reportId });
        }

        // POST: ReportItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ReportId,OrganizationName,Region,Address,CargoName,UnitTypeId,PlanedAmount,PlanedSum,FactAmount,FactSum,BalanceAmount,BalanceSum,ReserveAmount,ReserveSum")] ReportItem reportItem)
        {
            if (ModelState.IsValid)
            {
                db.ReportItems.Add(reportItem);
                db.SaveChanges();
                return RedirectToAction("Details", "Reports", new { id = reportItem.ReportId });
            }

            //ViewBag.ReportId = new SelectList(db.Reports, "Id", "UserId", reportItem.ReportId);
            //ViewBag.UnitTypeId = new SelectList(db.UnitTypes, "Id", "Name", reportItem.UnitTypeId);
            return View(reportItem);
        }

        // GET: ReportItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportItem reportItem = db.ReportItems.Find(id);
            if (reportItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReportId = new SelectList(db.Reports, "Id", "UserId", reportItem.ReportId);
            //ViewBag.UnitTypeId = new SelectList(db.UnitTypes, "Id", "Name", reportItem.UnitTypeId);
            return View(reportItem);
        }

        // POST: ReportItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ReportId,OrganizationName,Region,Address,CargoName,UnitTypeId,PlanedAmount,PlanedSum,FactAmount,FactSum,BalanceAmount,BalanceSum,ReserveAmount,ReserveSum")] ReportItem reportItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reportItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Reports", new { id = reportItem.ReportId });
            }
            //ViewBag.ReportId = new SelectList(db.Reports, "Id", "UserId", reportItem.ReportId);
            //ViewBag.UnitTypeId = new SelectList(db.UnitTypes, "Id", "Name", reportItem.UnitTypeId);
            return View(reportItem);
        }

        // GET: ReportItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportItem reportItem = db.ReportItems.Find(id);
            if (reportItem == null)
            {
                return HttpNotFound();
            }
            return View(reportItem);
        }

        // POST: ReportItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReportItem reportItem = db.ReportItems.Find(id);
            db.ReportItems.Remove(reportItem);
            db.SaveChanges();
            return RedirectToAction("Details", "Reports", new { id = reportItem.ReportId });
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
