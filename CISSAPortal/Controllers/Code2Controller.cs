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
    public class Code2Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Code2
        public ActionResult Index()
        {
            return View(db.Codes2.ToList());
        }

        // GET: Code2/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Code2 code2 = db.Codes2.Find(id);
            if (code2 == null)
            {
                return HttpNotFound();
            }
            return View(code2);
        }

        // GET: Code2/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Code2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,FullName,Description")] Code2 code2)
        {
            if (ModelState.IsValid)
            {
                db.Codes2.Add(code2);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(code2);
        }

        // GET: Code2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Code2 code2 = db.Codes2.Find(id);
            if (code2 == null)
            {
                return HttpNotFound();
            }
            return View(code2);
        }

        // POST: Code2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,FullName,Description")] Code2 code2)
        {
            if (ModelState.IsValid)
            {
                db.Entry(code2).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(code2);
        }

        // GET: Code2/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Code2 code2 = db.Codes2.Find(id);
            if (code2 == null)
            {
                return HttpNotFound();
            }
            return View(code2);
        }

        // POST: Code2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Code2 code2 = db.Codes2.Find(id);
            db.Codes2.Remove(code2);
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
