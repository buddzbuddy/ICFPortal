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
    public class ParentCasesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ParentCases
        public ActionResult Index()
        {
            var parentCases = db.ParentCases.Include(p => p.Patient).Include(p => p.Reference3).Include(p => p.Reference4_1_1).Include(p => p.Reference4_1_2).Include(p => p.Reference4_1_3).Include(p => p.Reference4_1_4).Include(p => p.Reference4_1_5).Include(p => p.Reference4_1_6).Include(p => p.Reference4_1_7).Include(p => p.Reference4_1_8).Include(p => p.Reference4_1_9).Include(p => p.Reference4_2_1).Include(p => p.Reference4_2_2).Include(p => p.Reference4_2_3).Include(p => p.Reference4_2_4).Include(p => p.Reference4_2_5).Include(p => p.Reference4_2_6).Include(p => p.Reference4_2_7).Include(p => p.Reference4_2_8).Include(p => p.Reference4_3_1).Include(p => p.Reference4_3_2).Include(p => p.Reference4_3_3).Include(p => p.Reference4_4_1).Include(p => p.Reference4_4_2).Include(p => p.Reference4_4_3).Include(p => p.Reference4_4_4).Include(p => p.Reference4_4_5).Include(p => p.Reference4_5_1).Include(p => p.Reference4_5_2).Include(p => p.Reference4_6_1).Include(p => p.Reference4_6_2).Include(p => p.Reference4_6_3).Include(p => p.Reference4_6_4).Include(p => p.Reference4_6_5).Include(p => p.Reference4_6_6).Include(p => p.Reference4_6_7).Include(p => p.Reference4_6_8).Include(p => p.Reference4_6_9);
            return View(parentCases.ToList());
        }

        // GET: ParentCases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParentCase parentCase = db.ParentCases.Find(id);
            if (parentCase == null)
            {
                return HttpNotFound();
            }
            parentCase.Patient = db.Patients.Find(parentCase.PatientId);
            ViewBag.Reference3Id = new SelectList(db.Reference3, "Id", "Name");
            ViewBag.Reference4Names = db.Reference4.OrderBy(x => x.Code).Select(x => x.Name).ToList();
            ViewData["Reference4_1_1Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_1_1Id);
            ViewData["Reference4_1_2Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_1_2Id);
            ViewData["Reference4_1_3Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_1_3Id);
            ViewData["Reference4_1_4Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_1_4Id);
            ViewData["Reference4_1_5Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_1_5Id);
            ViewData["Reference4_1_6Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_1_6Id);
            ViewData["Reference4_1_7Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_1_7Id);
            ViewData["Reference4_1_8Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_1_8Id);
            ViewData["Reference4_1_9Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_1_9Id);
            ViewData["Reference4_2_1Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_2_1Id);
            ViewData["Reference4_2_2Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_2_2Id);
            ViewData["Reference4_2_3Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_2_3Id);
            ViewData["Reference4_2_4Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_2_4Id);
            ViewData["Reference4_2_5Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_2_5Id);
            ViewData["Reference4_2_6Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_2_6Id);
            ViewData["Reference4_2_7Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_2_7Id);
            ViewData["Reference4_2_8Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_2_8Id);
            ViewData["Reference4_3_1Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_3_1Id);
            ViewData["Reference4_3_2Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_3_2Id);
            ViewData["Reference4_3_3Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_3_3Id);
            ViewData["Reference4_4_1Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_4_1Id);
            ViewData["Reference4_4_2Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_4_2Id);
            ViewData["Reference4_4_3Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_4_3Id);
            ViewData["Reference4_4_4Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_4_4Id);
            ViewData["Reference4_4_5Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_4_5Id);
            ViewData["Reference4_5_1Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_5_1Id);
            ViewData["Reference4_5_2Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_5_2Id);
            ViewData["Reference4_6_1Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_6_1Id);
            ViewData["Reference4_6_2Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_6_2Id);
            ViewData["Reference4_6_3Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_6_3Id);
            ViewData["Reference4_6_4Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_6_4Id);
            ViewData["Reference4_6_5Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_6_5Id);
            ViewData["Reference4_6_6Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_6_6Id);
            ViewData["Reference4_6_7Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_6_7Id);
            ViewData["Reference4_6_8Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_6_8Id);
            ViewData["Reference4_6_9Id"] = new SelectList(db.Reference4, "Id", "Code", parentCase.Reference4_6_9Id);
            return View(parentCase);
        }

        // GET: ParentCases/Create
        public ActionResult Create(int patientId)
        {
            var model = new ParentCase { PatientId = patientId, Patient = db.Patients.Find(patientId) };
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "LastName");
            ViewBag.Reference3Id = new SelectList(db.Reference3, "Id", "Name");
            ViewBag.Reference4Names = db.Reference4.OrderBy(x => x.Code).Select(x => x.Name).ToList();
            ViewData["Reference4_1_1Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_1_2Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_1_3Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_1_4Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_1_5Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_1_6Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_1_7Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_1_8Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_1_9Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_2_1Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_2_2Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_2_3Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_2_4Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_2_5Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_2_6Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_2_7Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_2_8Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_3_1Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_3_2Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_3_3Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_4_1Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_4_2Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_4_3Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_4_4Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_4_5Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_5_1Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_5_2Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_6_1Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_6_2Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_6_3Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_6_4Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_6_5Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_6_6Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_6_7Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_6_8Id"] = new SelectList(db.Reference4, "Id", "Code");
            ViewData["Reference4_6_9Id"] = new SelectList(db.Reference4, "Id", "Code");
            return View(model);
        }

        // POST: ParentCases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PatientId,FullName,FilledPlaceName,FilledDate,Reference3Id,Reference4_1_1Id,Reference4_1_2Id,Reference4_1_3Id,Reference4_1_4Id,Reference4_1_5Id,Reference4_1_6Id,Reference4_1_7Id,Reference4_1_8Id,Reference4_1_9Id,Reference4_2_1Id,Reference4_2_2Id,Reference4_2_3Id,Reference4_2_4Id,Reference4_2_5Id,Reference4_2_6Id,Reference4_2_7Id,Reference4_2_8Id,Reference4_3_1Id,Reference4_3_2Id,Reference4_3_3Id,Reference4_4_1Id,Reference4_4_2Id,Reference4_4_3Id,Reference4_4_4Id,Reference4_4_5Id,Reference4_5_1Id,Reference4_5_2Id,Reference4_6_1Id,Reference4_6_2Id,Reference4_6_3Id,Reference4_6_4Id,Reference4_6_5Id,Reference4_6_6Id,Reference4_6_7Id,Reference4_6_8Id,Reference4_6_9Id")] ParentCase parentCase)
        {
            if (ModelState.IsValid)
            {
                db.ParentCases.Add(parentCase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PatientId = new SelectList(db.Patients, "Id", "LastName", parentCase.PatientId);
            ViewBag.Reference3Id = new SelectList(db.Reference3, "Id", "Name", parentCase.Reference3Id);
            ViewBag.Reference4_1_1Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_1Id);
            ViewBag.Reference4_1_2Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_2Id);
            ViewBag.Reference4_1_3Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_3Id);
            ViewBag.Reference4_1_4Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_4Id);
            ViewBag.Reference4_1_5Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_5Id);
            ViewBag.Reference4_1_6Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_6Id);
            ViewBag.Reference4_1_7Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_7Id);
            ViewBag.Reference4_1_8Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_8Id);
            ViewBag.Reference4_1_9Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_9Id);
            ViewBag.Reference4_2_1Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_1Id);
            ViewBag.Reference4_2_2Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_2Id);
            ViewBag.Reference4_2_3Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_3Id);
            ViewBag.Reference4_2_4Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_4Id);
            ViewBag.Reference4_2_5Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_5Id);
            ViewBag.Reference4_2_6Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_6Id);
            ViewBag.Reference4_2_7Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_7Id);
            ViewBag.Reference4_2_8Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_8Id);
            ViewBag.Reference4_3_1Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_3_1Id);
            ViewBag.Reference4_3_2Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_3_2Id);
            ViewBag.Reference4_3_3Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_3_3Id);
            ViewBag.Reference4_4_1Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_4_1Id);
            ViewBag.Reference4_4_2Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_4_2Id);
            ViewBag.Reference4_4_3Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_4_3Id);
            ViewBag.Reference4_4_4Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_4_4Id);
            ViewBag.Reference4_4_5Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_4_5Id);
            ViewBag.Reference4_5_1Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_5_1Id);
            ViewBag.Reference4_5_2Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_5_2Id);
            ViewBag.Reference4_6_1Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_1Id);
            ViewBag.Reference4_6_2Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_2Id);
            ViewBag.Reference4_6_3Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_3Id);
            ViewBag.Reference4_6_4Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_4Id);
            ViewBag.Reference4_6_5Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_5Id);
            ViewBag.Reference4_6_6Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_6Id);
            ViewBag.Reference4_6_7Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_7Id);
            ViewBag.Reference4_6_8Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_8Id);
            ViewBag.Reference4_6_9Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_9Id);
            return View(parentCase);
        }

        // GET: ParentCases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParentCase parentCase = db.ParentCases.Find(id);
            if (parentCase == null)
            {
                return HttpNotFound();
            }
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "LastName", parentCase.PatientId);
            ViewBag.Reference3Id = new SelectList(db.Reference3, "Id", "Name", parentCase.Reference3Id);
            ViewBag.Reference4_1_1Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_1Id);
            ViewBag.Reference4_1_2Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_2Id);
            ViewBag.Reference4_1_3Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_3Id);
            ViewBag.Reference4_1_4Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_4Id);
            ViewBag.Reference4_1_5Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_5Id);
            ViewBag.Reference4_1_6Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_6Id);
            ViewBag.Reference4_1_7Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_7Id);
            ViewBag.Reference4_1_8Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_8Id);
            ViewBag.Reference4_1_9Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_9Id);
            ViewBag.Reference4_2_1Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_1Id);
            ViewBag.Reference4_2_2Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_2Id);
            ViewBag.Reference4_2_3Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_3Id);
            ViewBag.Reference4_2_4Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_4Id);
            ViewBag.Reference4_2_5Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_5Id);
            ViewBag.Reference4_2_6Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_6Id);
            ViewBag.Reference4_2_7Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_7Id);
            ViewBag.Reference4_2_8Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_8Id);
            ViewBag.Reference4_3_1Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_3_1Id);
            ViewBag.Reference4_3_2Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_3_2Id);
            ViewBag.Reference4_3_3Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_3_3Id);
            ViewBag.Reference4_4_1Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_4_1Id);
            ViewBag.Reference4_4_2Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_4_2Id);
            ViewBag.Reference4_4_3Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_4_3Id);
            ViewBag.Reference4_4_4Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_4_4Id);
            ViewBag.Reference4_4_5Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_4_5Id);
            ViewBag.Reference4_5_1Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_5_1Id);
            ViewBag.Reference4_5_2Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_5_2Id);
            ViewBag.Reference4_6_1Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_1Id);
            ViewBag.Reference4_6_2Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_2Id);
            ViewBag.Reference4_6_3Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_3Id);
            ViewBag.Reference4_6_4Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_4Id);
            ViewBag.Reference4_6_5Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_5Id);
            ViewBag.Reference4_6_6Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_6Id);
            ViewBag.Reference4_6_7Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_7Id);
            ViewBag.Reference4_6_8Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_8Id);
            ViewBag.Reference4_6_9Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_9Id);
            return View(parentCase);
        }

        // POST: ParentCases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PatientId,FullName,FilledPlaceName,FilledDate,Reference3Id,Reference4_1_1Id,Reference4_1_2Id,Reference4_1_3Id,Reference4_1_4Id,Reference4_1_5Id,Reference4_1_6Id,Reference4_1_7Id,Reference4_1_8Id,Reference4_1_9Id,Reference4_2_1Id,Reference4_2_2Id,Reference4_2_3Id,Reference4_2_4Id,Reference4_2_5Id,Reference4_2_6Id,Reference4_2_7Id,Reference4_2_8Id,Reference4_3_1Id,Reference4_3_2Id,Reference4_3_3Id,Reference4_4_1Id,Reference4_4_2Id,Reference4_4_3Id,Reference4_4_4Id,Reference4_4_5Id,Reference4_5_1Id,Reference4_5_2Id,Reference4_6_1Id,Reference4_6_2Id,Reference4_6_3Id,Reference4_6_4Id,Reference4_6_5Id,Reference4_6_6Id,Reference4_6_7Id,Reference4_6_8Id,Reference4_6_9Id")] ParentCase parentCase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parentCase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "LastName", parentCase.PatientId);
            ViewBag.Reference3Id = new SelectList(db.Reference3, "Id", "Name", parentCase.Reference3Id);
            ViewBag.Reference4_1_1Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_1Id);
            ViewBag.Reference4_1_2Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_2Id);
            ViewBag.Reference4_1_3Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_3Id);
            ViewBag.Reference4_1_4Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_4Id);
            ViewBag.Reference4_1_5Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_5Id);
            ViewBag.Reference4_1_6Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_6Id);
            ViewBag.Reference4_1_7Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_7Id);
            ViewBag.Reference4_1_8Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_8Id);
            ViewBag.Reference4_1_9Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_1_9Id);
            ViewBag.Reference4_2_1Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_1Id);
            ViewBag.Reference4_2_2Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_2Id);
            ViewBag.Reference4_2_3Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_3Id);
            ViewBag.Reference4_2_4Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_4Id);
            ViewBag.Reference4_2_5Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_5Id);
            ViewBag.Reference4_2_6Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_6Id);
            ViewBag.Reference4_2_7Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_7Id);
            ViewBag.Reference4_2_8Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_2_8Id);
            ViewBag.Reference4_3_1Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_3_1Id);
            ViewBag.Reference4_3_2Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_3_2Id);
            ViewBag.Reference4_3_3Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_3_3Id);
            ViewBag.Reference4_4_1Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_4_1Id);
            ViewBag.Reference4_4_2Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_4_2Id);
            ViewBag.Reference4_4_3Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_4_3Id);
            ViewBag.Reference4_4_4Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_4_4Id);
            ViewBag.Reference4_4_5Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_4_5Id);
            ViewBag.Reference4_5_1Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_5_1Id);
            ViewBag.Reference4_5_2Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_5_2Id);
            ViewBag.Reference4_6_1Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_1Id);
            ViewBag.Reference4_6_2Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_2Id);
            ViewBag.Reference4_6_3Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_3Id);
            ViewBag.Reference4_6_4Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_4Id);
            ViewBag.Reference4_6_5Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_5Id);
            ViewBag.Reference4_6_6Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_6Id);
            ViewBag.Reference4_6_7Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_7Id);
            ViewBag.Reference4_6_8Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_8Id);
            ViewBag.Reference4_6_9Id = new SelectList(db.Reference4, "Id", "Name", parentCase.Reference4_6_9Id);
            return View(parentCase);
        }

        // GET: ParentCases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParentCase parentCase = db.ParentCases.Find(id);
            if (parentCase == null)
            {
                return HttpNotFound();
            }
            return View(parentCase);
        }

        // POST: ParentCases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParentCase parentCase = db.ParentCases.Find(id);
            db.ParentCases.Remove(parentCase);
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
