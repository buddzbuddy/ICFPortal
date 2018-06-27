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
    public class Code4Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Code4
        public ActionResult Index()
        {
            return View(db.Codes4.ToList());
        }

        // GET: Code4/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Code4 code4 = db.Codes4.Find(id);
            if (code4 == null)
            {
                return HttpNotFound();
            }
            return View(code4);
        }

        // GET: Code4/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Code4/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,FullName,Description")] Code4 code4)
        {
            if (ModelState.IsValid)
            {
                db.Codes4.Add(code4);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(code4);
        }

        // GET: Code4/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Code4 code4 = db.Codes4.Find(id);
            if (code4 == null)
            {
                return HttpNotFound();
            }
            return View(code4);
        }

        // POST: Code4/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,FullName,Description")] Code4 code4)
        {
            if (ModelState.IsValid)
            {
                db.Entry(code4).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(code4);
        }

        // GET: Code4/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Code4 code4 = db.Codes4.Find(id);
            if (code4 == null)
            {
                return HttpNotFound();
            }
            return View(code4);
        }

        // POST: Code4/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Code4 code4 = db.Codes4.Find(id);
            db.Codes4.Remove(code4);
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
