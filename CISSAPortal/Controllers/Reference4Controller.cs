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
    public class Reference4Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reference4
        public ActionResult Index()
        {
            return View(db.Reference4.ToList());
        }

        // GET: Reference4/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reference4 reference4 = db.Reference4.Find(id);
            if (reference4 == null)
            {
                return HttpNotFound();
            }
            return View(reference4);
        }

        // GET: Reference4/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reference4/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Reference4 reference4)
        {
            if (ModelState.IsValid)
            {
                db.Reference4.Add(reference4);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reference4);
        }

        // GET: Reference4/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reference4 reference4 = db.Reference4.Find(id);
            if (reference4 == null)
            {
                return HttpNotFound();
            }
            return View(reference4);
        }

        // POST: Reference4/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Reference4 reference4)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reference4).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reference4);
        }

        // GET: Reference4/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reference4 reference4 = db.Reference4.Find(id);
            if (reference4 == null)
            {
                return HttpNotFound();
            }
            return View(reference4);
        }

        // POST: Reference4/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reference4 reference4 = db.Reference4.Find(id);
            db.Reference4.Remove(reference4);
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
