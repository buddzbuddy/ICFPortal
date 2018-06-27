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
    public class Code1Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Code1
        public ActionResult Index()
        {
            return View(db.Codes1.ToList());
        }

        // GET: Code1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Code1 code1 = db.Codes1.Find(id);
            if (code1 == null)
            {
                return HttpNotFound();
            }
            return View(code1);
        }

        // GET: Code1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Code1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,FullName,Description")] Code1 code1)
        {
            var objDb = db.Codes1.FirstOrDefault(x => x.Name == code1.Name);
            if (objDb != null)
            {

            }
            if (ModelState.IsValid)
            {
                db.Codes1.Add(code1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(code1);
        }

        // GET: Code1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Code1 code1 = db.Codes1.Find(id);
            if (code1 == null)
            {
                return HttpNotFound();
            }
            return View(code1);
        }

        // POST: Code1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,FullName,Description")] Code1 code1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(code1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(code1);
        }

        // GET: Code1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Code1 code1 = db.Codes1.Find(id);
            if (code1 == null)
            {
                return HttpNotFound();
            }
            return View(code1);
        }

        // POST: Code1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Code1 code1 = db.Codes1.Find(id);
            db.Codes1.Remove(code1);
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
