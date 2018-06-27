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
    public class Code3Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Code3
        public ActionResult Index()
        {
            return View(db.Codes3.ToList());
        }

        // GET: Code3/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Code3 code3 = db.Codes3.Find(id);
            if (code3 == null)
            {
                return HttpNotFound();
            }
            return View(code3);
        }

        // GET: Code3/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Code3/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,FullName,Description")] Code3 code3)
        {
            if (ModelState.IsValid)
            {
                db.Codes3.Add(code3);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(code3);
        }

        // GET: Code3/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Code3 code3 = db.Codes3.Find(id);
            if (code3 == null)
            {
                return HttpNotFound();
            }
            return View(code3);
        }

        // POST: Code3/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,FullName,Description")] Code3 code3)
        {
            if (ModelState.IsValid)
            {
                db.Entry(code3).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(code3);
        }

        // GET: Code3/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Code3 code3 = db.Codes3.Find(id);
            if (code3 == null)
            {
                return HttpNotFound();
            }
            return View(code3);
        }

        // POST: Code3/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Code3 code3 = db.Codes3.Find(id);
            db.Codes3.Remove(code3);
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
