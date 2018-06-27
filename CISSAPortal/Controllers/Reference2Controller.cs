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
    public class Reference2Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reference2
        public ActionResult Index()
        {
            return View(db.Reference2.ToList());
        }

        // GET: Reference2/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reference2 reference2 = db.Reference2.Find(id);
            if (reference2 == null)
            {
                return HttpNotFound();
            }
            return View(reference2);
        }

        // GET: Reference2/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reference2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Reference2 reference2)
        {
            if (ModelState.IsValid)
            {
                db.Reference2.Add(reference2);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reference2);
        }

        // GET: Reference2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reference2 reference2 = db.Reference2.Find(id);
            if (reference2 == null)
            {
                return HttpNotFound();
            }
            return View(reference2);
        }

        // POST: Reference2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Reference2 reference2)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reference2).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reference2);
        }

        // GET: Reference2/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reference2 reference2 = db.Reference2.Find(id);
            if (reference2 == null)
            {
                return HttpNotFound();
            }
            return View(reference2);
        }

        // POST: Reference2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reference2 reference2 = db.Reference2.Find(id);
            db.Reference2.Remove(reference2);
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
