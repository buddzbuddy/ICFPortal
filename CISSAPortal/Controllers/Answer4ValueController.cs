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
    public class Answer4ValueController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Answer4Value
        public ActionResult Index()
        {
            return View(db.Answer4Values.ToList());
        }

        // GET: Answer4Value/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer4Value answer4Value = db.Answer4Values.Find(id);
            if (answer4Value == null)
            {
                return HttpNotFound();
            }
            return View(answer4Value);
        }

        // GET: Answer4Value/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Answer4Value/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Answer4Value answer4Value)
        {
            if (ModelState.IsValid)
            {
                db.Answer4Values.Add(answer4Value);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(answer4Value);
        }

        // GET: Answer4Value/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer4Value answer4Value = db.Answer4Values.Find(id);
            if (answer4Value == null)
            {
                return HttpNotFound();
            }
            return View(answer4Value);
        }

        // POST: Answer4Value/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Answer4Value answer4Value)
        {
            if (ModelState.IsValid)
            {
                db.Entry(answer4Value).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(answer4Value);
        }

        // GET: Answer4Value/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer4Value answer4Value = db.Answer4Values.Find(id);
            if (answer4Value == null)
            {
                return HttpNotFound();
            }
            return View(answer4Value);
        }

        // POST: Answer4Value/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Answer4Value answer4Value = db.Answer4Values.Find(id);
            db.Answer4Values.Remove(answer4Value);
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
