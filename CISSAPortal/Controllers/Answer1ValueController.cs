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
    public class Answer1ValueController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Answer1Value
        public ActionResult Index()
        {
            return View(db.Answer1Values.ToList());
        }

        // GET: Answer1Value/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer1Value answer1Value = db.Answer1Values.Find(id);
            if (answer1Value == null)
            {
                return HttpNotFound();
            }
            return View(answer1Value);
        }

        // GET: Answer1Value/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Answer1Value/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Answer1Value answer1Value)
        {
            if (ModelState.IsValid)
            {
                db.Answer1Values.Add(answer1Value);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(answer1Value);
        }

        // GET: Answer1Value/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer1Value answer1Value = db.Answer1Values.Find(id);
            if (answer1Value == null)
            {
                return HttpNotFound();
            }
            return View(answer1Value);
        }

        // POST: Answer1Value/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Answer1Value answer1Value)
        {
            if (ModelState.IsValid)
            {
                db.Entry(answer1Value).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(answer1Value);
        }

        // GET: Answer1Value/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer1Value answer1Value = db.Answer1Values.Find(id);
            if (answer1Value == null)
            {
                return HttpNotFound();
            }
            return View(answer1Value);
        }

        // POST: Answer1Value/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Answer1Value answer1Value = db.Answer1Values.Find(id);
            db.Answer1Values.Remove(answer1Value);
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
