﻿using System;
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
    [Authorize]
    public class UnitTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UnitTypes
        public ActionResult Index()
        {
            return View(db.UnitTypes.ToList());
        }
        // GET: UnitTypes
        public ActionResult IndexTS()
        {
            return View();
        }

        public JsonResult GetUnitTypesTS()
        {
            return Json(db.UnitTypes.ToList(), JsonRequestBehavior.AllowGet);
        }

        // GET: UnitTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitType unitType = db.UnitTypes.Find(id);
            if (unitType == null)
            {
                return HttpNotFound();
            }
            return View(unitType);
        }

        // GET: UnitTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UnitTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] UnitType unitType)
        {
            if (ModelState.IsValid)
            {
                db.UnitTypes.Add(unitType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(unitType);
        }

        // GET: UnitTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitType unitType = db.UnitTypes.Find(id);
            if (unitType == null)
            {
                return HttpNotFound();
            }
            return View(unitType);
        }

        // POST: UnitTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] UnitType unitType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unitType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(unitType);
        }

        // GET: UnitTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitType unitType = db.UnitTypes.Find(id);
            if (unitType == null)
            {
                return HttpNotFound();
            }
            return View(unitType);
        }

        // POST: UnitTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UnitType unitType = db.UnitTypes.Find(id);
            db.UnitTypes.Remove(unitType);
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
