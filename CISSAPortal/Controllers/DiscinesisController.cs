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
    public class DiscinesisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Discinesis
        public ActionResult Index()
        {
            return View(db.Discinesis.ToList());
        }

        // GET: Discinesis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discinesis discinesis = db.Discinesis.Find(id);
            if (discinesis == null)
            {
                return HttpNotFound();
            }
            return View(discinesis);
        }

        // GET: Discinesis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Discinesis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Discinesis discinesis)
        {
            if (ModelState.IsValid)
            {
                db.Discinesis.Add(discinesis);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(discinesis);
        }

        // GET: Discinesis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discinesis discinesis = db.Discinesis.Find(id);
            if (discinesis == null)
            {
                return HttpNotFound();
            }
            return View(discinesis);
        }

        // POST: Discinesis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Discinesis discinesis)
        {
            if (ModelState.IsValid)
            {
                db.Entry(discinesis).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(discinesis);
        }

        // GET: Discinesis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discinesis discinesis = db.Discinesis.Find(id);
            if (discinesis == null)
            {
                return HttpNotFound();
            }
            return View(discinesis);
        }

        // POST: Discinesis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Discinesis discinesis = db.Discinesis.Find(id);
            db.Discinesis.Remove(discinesis);
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
