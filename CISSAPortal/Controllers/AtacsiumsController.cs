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
    public class AtacsiumsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Atacsiums
        public ActionResult Index()
        {
            return View(db.Atacsiums.ToList());
        }

        // GET: Atacsiums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atacsium atacsium = db.Atacsiums.Find(id);
            if (atacsium == null)
            {
                return HttpNotFound();
            }
            return View(atacsium);
        }

        // GET: Atacsiums/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Atacsiums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Atacsium atacsium)
        {
            if (ModelState.IsValid)
            {
                db.Atacsiums.Add(atacsium);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(atacsium);
        }

        // GET: Atacsiums/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atacsium atacsium = db.Atacsiums.Find(id);
            if (atacsium == null)
            {
                return HttpNotFound();
            }
            return View(atacsium);
        }

        // POST: Atacsiums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Atacsium atacsium)
        {
            if (ModelState.IsValid)
            {
                db.Entry(atacsium).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(atacsium);
        }

        // GET: Atacsiums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atacsium atacsium = db.Atacsiums.Find(id);
            if (atacsium == null)
            {
                return HttpNotFound();
            }
            return View(atacsium);
        }

        // POST: Atacsiums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Atacsium atacsium = db.Atacsiums.Find(id);
            db.Atacsiums.Remove(atacsium);
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
