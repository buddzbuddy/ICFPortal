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
    public class Answer2Value1Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Answer2Value1
        public ActionResult Index()
        {
            return View(db.Answer2Values1.ToList());
        }

        // GET: Answer2Value1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer2Value1 answer2Value1 = db.Answer2Values1.Find(id);
            if (answer2Value1 == null)
            {
                return HttpNotFound();
            }
            return View(answer2Value1);
        }

        // GET: Answer2Value1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Answer2Value1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Answer2Value1 answer2Value1)
        {
            if (ModelState.IsValid)
            {
                db.Answer2Values1.Add(answer2Value1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(answer2Value1);
        }

        // GET: Answer2Value1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer2Value1 answer2Value1 = db.Answer2Values1.Find(id);
            if (answer2Value1 == null)
            {
                return HttpNotFound();
            }
            return View(answer2Value1);
        }

        // POST: Answer2Value1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Answer2Value1 answer2Value1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(answer2Value1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(answer2Value1);
        }

        // GET: Answer2Value1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer2Value1 answer2Value1 = db.Answer2Values1.Find(id);
            if (answer2Value1 == null)
            {
                return HttpNotFound();
            }
            return View(answer2Value1);
        }

        // POST: Answer2Value1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Answer2Value1 answer2Value1 = db.Answer2Values1.Find(id);
            db.Answer2Values1.Remove(answer2Value1);
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
