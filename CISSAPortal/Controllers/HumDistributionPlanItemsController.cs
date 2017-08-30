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
    public class HumDistributionPlanItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HumDistributionPlanItems
        public ActionResult Index()
        {
            var humDistributionPlanItems = db.HumDistributionPlanItems.Include(h => h.HumDistributionPlan).Include(h => h.UnitType);
            return View(humDistributionPlanItems.ToList());
        }

        // GET: HumDistributionPlanItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HumDistributionPlanItem humDistributionPlanItem = db.HumDistributionPlanItems.Find(id);
            if (humDistributionPlanItem == null)
            {
                return HttpNotFound();
            }
            return View(humDistributionPlanItem);
        }

        // GET: HumDistributionPlanItems/Create
        public ActionResult Create(int HumDistributionPlanId)
        {
            //ViewBag.HumDistributionPlanId = HumDistributionPlanId;//new SelectList(db.HumDistributionPlans, "Id", "Id");
            ViewBag.UnitTypeId = new SelectList(db.UnitTypes, "Id", "Name");
            return View(new HumDistributionPlanItem { HumDistributionPlanId = HumDistributionPlanId });
        }

        // POST: HumDistributionPlanItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HumDistributionPlanId,Consumer,Region,Address,ProductName,UnitTypeId,Amount,Weight,Sum")] HumDistributionPlanItem humDistributionPlanItem)
        {
            if (ModelState.IsValid)
            {
                db.HumDistributionPlanItems.Add(humDistributionPlanItem);
                db.SaveChanges();
                return RedirectToAction("Details", "HumDistributionPlans", new { id = humDistributionPlanItem.HumDistributionPlanId });//return RedirectToAction("Index");
            }

            ViewBag.HumDistributionPlanId = new SelectList(db.HumDistributionPlans, "Id", "Id", humDistributionPlanItem.HumDistributionPlanId);
            ViewBag.UnitTypeId = new SelectList(db.UnitTypes, "Id", "Name", humDistributionPlanItem.UnitTypeId);
            return View(humDistributionPlanItem);
        }

        // GET: HumDistributionPlanItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HumDistributionPlanItem humDistributionPlanItem = db.HumDistributionPlanItems.Find(id);
            if (humDistributionPlanItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.HumDistributionPlanId = new SelectList(db.HumDistributionPlans, "Id", "Id", humDistributionPlanItem.HumDistributionPlanId);
            ViewBag.UnitTypeId = new SelectList(db.UnitTypes, "Id", "Name", humDistributionPlanItem.UnitTypeId);
            return View(humDistributionPlanItem);
        }

        // POST: HumDistributionPlanItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HumDistributionPlanId,Consumer,Region,Address,ProductName,UnitTypeId,Amount,Weight,Sum")] HumDistributionPlanItem humDistributionPlanItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(humDistributionPlanItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HumDistributionPlanId = new SelectList(db.HumDistributionPlans, "Id", "Id", humDistributionPlanItem.HumDistributionPlanId);
            ViewBag.UnitTypeId = new SelectList(db.UnitTypes, "Id", "Name", humDistributionPlanItem.UnitTypeId);
            return View(humDistributionPlanItem);
        }

        // GET: HumDistributionPlanItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HumDistributionPlanItem humDistributionPlanItem = db.HumDistributionPlanItems.Find(id);
            if (humDistributionPlanItem == null)
            {
                return HttpNotFound();
            }
            return View(humDistributionPlanItem);
        }

        // POST: HumDistributionPlanItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HumDistributionPlanItem humDistributionPlanItem = db.HumDistributionPlanItems.Find(id);
            db.HumDistributionPlanItems.Remove(humDistributionPlanItem);
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
}
