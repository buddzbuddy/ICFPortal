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
    public class Answer3Value2Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Answer3Value2
        public ActionResult Index()
        {
            return View(db.Answer3Values2.ToList());
        }

        // GET: Answer3Value2/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer3Value2 answer3Value2 = db.Answer3Values2.Find(id);
            if (answer3Value2 == null)
            {
                return HttpNotFound();
            }
            return View(answer3Value2);
        }

        // GET: Answer3Value2/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Answer3Value2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Answer3Value2 answer3Value2)
        {
            if (ModelState.IsValid)
            {
                db.Answer3Values2.Add(answer3Value2);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(answer3Value2);
        }

        // GET: Answer3Value2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer3Value2 answer3Value2 = db.Answer3Values2.Find(id);
            if (answer3Value2 == null)
            {
                return HttpNotFound();
            }
            return View(answer3Value2);
        }

        // POST: Answer3Value2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Answer3Value2 answer3Value2)
        {
            if (ModelState.IsValid)
            {
                db.Entry(answer3Value2).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(answer3Value2);
        }

        // GET: Answer3Value2/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer3Value2 answer3Value2 = db.Answer3Values2.Find(id);
            if (answer3Value2 == null)
            {
                return HttpNotFound();
            }
            return View(answer3Value2);
        }

        // POST: Answer3Value2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Answer3Value2 answer3Value2 = db.Answer3Values2.Find(id);
            db.Answer3Values2.Remove(answer3Value2);
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
