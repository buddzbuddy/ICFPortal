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
    public class Reference3Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reference3
        public ActionResult Index()
        {
            return View(db.Reference3.ToList());
        }

        // GET: Reference3/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reference3 reference3 = db.Reference3.Find(id);
            if (reference3 == null)
            {
                return HttpNotFound();
            }
            return View(reference3);
        }

        // GET: Reference3/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reference3/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Reference3 reference3)
        {
            if (ModelState.IsValid)
            {
                db.Reference3.Add(reference3);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reference3);
        }

        // GET: Reference3/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reference3 reference3 = db.Reference3.Find(id);
            if (reference3 == null)
            {
                return HttpNotFound();
            }
            return View(reference3);
        }

        // POST: Reference3/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Reference3 reference3)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reference3).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reference3);
        }

        // GET: Reference3/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reference3 reference3 = db.Reference3.Find(id);
            if (reference3 == null)
            {
                return HttpNotFound();
            }
            return View(reference3);
        }

        // POST: Reference3/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reference3 reference3 = db.Reference3.Find(id);
            db.Reference3.Remove(reference3);
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
