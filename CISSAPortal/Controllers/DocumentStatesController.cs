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
    public class DocumentStatesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DocumentStates
        public ActionResult Index()
        {
            return View(db.DocumentStates.ToList());
        }

        // GET: DocumentStates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentState documentState = db.DocumentStates.Find(id);
            if (documentState == null)
            {
                return HttpNotFound();
            }
            return View(documentState);
        }

        // GET: DocumentStates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DocumentStates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Code")] DocumentState documentState)
        {
            if (ModelState.IsValid)
            {
                db.DocumentStates.Add(documentState);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(documentState);
        }

        // GET: DocumentStates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentState documentState = db.DocumentStates.Find(id);
            if (documentState == null)
            {
                return HttpNotFound();
            }
            return View(documentState);
        }

        // POST: DocumentStates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Code")] DocumentState documentState)
        {
            if (ModelState.IsValid)
            {
                db.Entry(documentState).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(documentState);
        }

        // GET: DocumentStates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentState documentState = db.DocumentStates.Find(id);
            if (documentState == null)
            {
                return HttpNotFound();
            }
            return View(documentState);
        }

        // POST: DocumentStates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DocumentState documentState = db.DocumentStates.Find(id);
            db.DocumentStates.Remove(documentState);
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
