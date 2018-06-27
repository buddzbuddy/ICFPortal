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
    public class Reference1Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reference1
        public ActionResult Index()
        {
            return View(db.Reference1.ToList());
        }

        // GET: Reference1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reference1 reference1 = db.Reference1.Find(id);
            if (reference1 == null)
            {
                return HttpNotFound();
            }
            return View(reference1);
        }

        // GET: Reference1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reference1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Reference1 reference1)
        {
            if (ModelState.IsValid)
            {
                db.Reference1.Add(reference1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reference1);
        }

        // GET: Reference1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reference1 reference1 = db.Reference1.Find(id);
            if (reference1 == null)
            {
                return HttpNotFound();
            }
            return View(reference1);
        }

        // POST: Reference1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Reference1 reference1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reference1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reference1);
        }

        // GET: Reference1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reference1 reference1 = db.Reference1.Find(id);
            if (reference1 == null)
            {
                return HttpNotFound();
            }
            return View(reference1);
        }

        // POST: Reference1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reference1 reference1 = db.Reference1.Find(id);
            db.Reference1.Remove(reference1);
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
