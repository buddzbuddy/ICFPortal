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
    public class CodeCaseBindingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CodeCaseBindings
        public ActionResult Index()
        {
            var codeCaseBindings = db.CodeCaseBindings.Include(c => c.Case);
            return View(codeCaseBindings.ToList());
        }

        // GET: CodeCaseBindings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodeCaseBinding codeCaseBinding = db.CodeCaseBindings.Find(id);
            if (codeCaseBinding == null)
            {
                return HttpNotFound();
            }
            return View(codeCaseBinding);
        }

        // GET: CodeCaseBindings/Create
        public ActionResult Create()
        {
            ViewBag.CaseId = new SelectList(db.Cases, "Id", "Name");
            return View();
        }

        // POST: CodeCaseBindings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CaseId,Code")] CodeCaseBinding codeCaseBinding)
        {
            if (ModelState.IsValid)
            {
                db.CodeCaseBindings.Add(codeCaseBinding);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CaseId = new SelectList(db.Cases, "Id", "Name", codeCaseBinding.CaseId);
            return View(codeCaseBinding);
        }

        // GET: CodeCaseBindings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodeCaseBinding codeCaseBinding = db.CodeCaseBindings.Find(id);
            if (codeCaseBinding == null)
            {
                return HttpNotFound();
            }
            ViewBag.CaseId = new SelectList(db.Cases, "Id", "Name", codeCaseBinding.CaseId);
            return View(codeCaseBinding);
        }

        // POST: CodeCaseBindings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CaseId,Code")] CodeCaseBinding codeCaseBinding)
        {
            if (ModelState.IsValid)
            {
                db.Entry(codeCaseBinding).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CaseId = new SelectList(db.Cases, "Id", "Name", codeCaseBinding.CaseId);
            return View(codeCaseBinding);
        }

        // GET: CodeCaseBindings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodeCaseBinding codeCaseBinding = db.CodeCaseBindings.Find(id);
            if (codeCaseBinding == null)
            {
                return HttpNotFound();
            }
            return View(codeCaseBinding);
        }

        // POST: CodeCaseBindings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CodeCaseBinding codeCaseBinding = db.CodeCaseBindings.Find(id);
            db.CodeCaseBindings.Remove(codeCaseBinding);
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
