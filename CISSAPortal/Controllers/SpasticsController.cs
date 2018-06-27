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
    public class SpasticsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Spastics
        public ActionResult Index()
        {
            return View(db.Spastics.ToList());
        }

        // GET: Spastics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spastic spastic = db.Spastics.Find(id);
            if (spastic == null)
            {
                return HttpNotFound();
            }
            return View(spastic);
        }

        // GET: Spastics/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Spastics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Spastic spastic)
        {
            if (ModelState.IsValid)
            {
                db.Spastics.Add(spastic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(spastic);
        }

        // GET: Spastics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spastic spastic = db.Spastics.Find(id);
            if (spastic == null)
            {
                return HttpNotFound();
            }
            return View(spastic);
        }

        // POST: Spastics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Spastic spastic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(spastic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(spastic);
        }

        // GET: Spastics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spastic spastic = db.Spastics.Find(id);
            if (spastic == null)
            {
                return HttpNotFound();
            }
            return View(spastic);
        }

        // POST: Spastics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Spastic spastic = db.Spastics.Find(id);
            db.Spastics.Remove(spastic);
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
