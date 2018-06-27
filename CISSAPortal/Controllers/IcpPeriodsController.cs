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
    public class IcpPeriodsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: IcpPeriods
        public ActionResult Index()
        {
            return View(db.IcpPeriods.ToList());
        }

        // GET: IcpPeriods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IcpPeriod icpPeriod = db.IcpPeriods.Find(id);
            if (icpPeriod == null)
            {
                return HttpNotFound();
            }
            return View(icpPeriod);
        }

        // GET: IcpPeriods/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IcpPeriods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] IcpPeriod icpPeriod)
        {
            if (ModelState.IsValid)
            {
                db.IcpPeriods.Add(icpPeriod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(icpPeriod);
        }

        // GET: IcpPeriods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IcpPeriod icpPeriod = db.IcpPeriods.Find(id);
            if (icpPeriod == null)
            {
                return HttpNotFound();
            }
            return View(icpPeriod);
        }

        // POST: IcpPeriods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] IcpPeriod icpPeriod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(icpPeriod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(icpPeriod);
        }

        // GET: IcpPeriods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IcpPeriod icpPeriod = db.IcpPeriods.Find(id);
            if (icpPeriod == null)
            {
                return HttpNotFound();
            }
            return View(icpPeriod);
        }

        // POST: IcpPeriods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IcpPeriod icpPeriod = db.IcpPeriods.Find(id);
            db.IcpPeriods.Remove(icpPeriod);
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
