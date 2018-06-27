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
    public class Answer3Value1Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Answer3Value1
        public ActionResult Index()
        {
            return View(db.Answer3Values1.ToList());
        }

        // GET: Answer3Value1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer3Value1 answer3Value1 = db.Answer3Values1.Find(id);
            if (answer3Value1 == null)
            {
                return HttpNotFound();
            }
            return View(answer3Value1);
        }

        // GET: Answer3Value1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Answer3Value1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Answer3Value1 answer3Value1)
        {
            if (ModelState.IsValid)
            {
                db.Answer3Values1.Add(answer3Value1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(answer3Value1);
        }

        // GET: Answer3Value1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer3Value1 answer3Value1 = db.Answer3Values1.Find(id);
            if (answer3Value1 == null)
            {
                return HttpNotFound();
            }
            return View(answer3Value1);
        }

        // POST: Answer3Value1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Answer3Value1 answer3Value1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(answer3Value1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(answer3Value1);
        }

        // GET: Answer3Value1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer3Value1 answer3Value1 = db.Answer3Values1.Find(id);
            if (answer3Value1 == null)
            {
                return HttpNotFound();
            }
            return View(answer3Value1);
        }

        // POST: Answer3Value1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Answer3Value1 answer3Value1 = db.Answer3Values1.Find(id);
            db.Answer3Values1.Remove(answer3Value1);
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
