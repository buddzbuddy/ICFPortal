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
    public class EpilepciesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Epilepcies
        public ActionResult Index()
        {
            return View(db.Epilepcies.ToList());
        }

        // GET: Epilepcies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Epilepcy epilepcy = db.Epilepcies.Find(id);
            if (epilepcy == null)
            {
                return HttpNotFound();
            }
            return View(epilepcy);
        }

        // GET: Epilepcies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Epilepcies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Epilepcy epilepcy)
        {
            if (ModelState.IsValid)
            {
                db.Epilepcies.Add(epilepcy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(epilepcy);
        }

        // GET: Epilepcies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Epilepcy epilepcy = db.Epilepcies.Find(id);
            if (epilepcy == null)
            {
                return HttpNotFound();
            }
            return View(epilepcy);
        }

        // POST: Epilepcies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Epilepcy epilepcy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(epilepcy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(epilepcy);
        }

        // GET: Epilepcies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Epilepcy epilepcy = db.Epilepcies.Find(id);
            if (epilepcy == null)
            {
                return HttpNotFound();
            }
            return View(epilepcy);
        }

        // POST: Epilepcies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Epilepcy epilepcy = db.Epilepcies.Find(id);
            db.Epilepcies.Remove(epilepcy);
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
