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
    public class IcpReasonsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: IcpReasons
        public ActionResult Index()
        {
            return View(db.IcpReasons.ToList());
        }

        // GET: IcpReasons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IcpReason icpReason = db.IcpReasons.Find(id);
            if (icpReason == null)
            {
                return HttpNotFound();
            }
            return View(icpReason);
        }

        // GET: IcpReasons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IcpReasons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] IcpReason icpReason)
        {
            if (ModelState.IsValid)
            {
                db.IcpReasons.Add(icpReason);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(icpReason);
        }

        // GET: IcpReasons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IcpReason icpReason = db.IcpReasons.Find(id);
            if (icpReason == null)
            {
                return HttpNotFound();
            }
            return View(icpReason);
        }

        // POST: IcpReasons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] IcpReason icpReason)
        {
            if (ModelState.IsValid)
            {
                db.Entry(icpReason).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(icpReason);
        }

        // GET: IcpReasons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IcpReason icpReason = db.IcpReasons.Find(id);
            if (icpReason == null)
            {
                return HttpNotFound();
            }
            return View(icpReason);
        }

        // POST: IcpReasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IcpReason icpReason = db.IcpReasons.Find(id);
            db.IcpReasons.Remove(icpReason);
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
