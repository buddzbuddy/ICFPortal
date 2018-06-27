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
    public class PatientsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Patients
        public ActionResult Index()
        {
            var patients = db.Patients.Include(p => p.Atacsium).Include(p => p.Discinesis).Include(p => p.Epilepcy).Include(p => p.IcpPeriod).Include(p => p.IcpReason).Include(p => p.Spastic);
            return View(patients.ToList());
        }

        // GET: Patients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            ViewBag.AtacsiumId = new SelectList(db.Atacsiums, "Id", "Name");
            ViewBag.DiscinesisId = new SelectList(db.Discinesis, "Id", "Name");
            ViewBag.EpilepcyId = new SelectList(db.Epilepcies, "Id", "Name");
            ViewBag.IcpPeriodId = new SelectList(db.IcpPeriods, "Id", "Name");
            ViewBag.IcpReasonId = new SelectList(db.IcpReasons, "Id", "Name");
            ViewBag.SpasticId = new SelectList(db.Spastics, "Id", "Name");
            ViewBag.Reference1Id = new SelectList(db.Reference1, "Id", "Name");
            ViewBag.Reference2Id = new SelectList(db.Reference2, "Id", "Name");
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,LastName,FirstName,MiddleName,Contacts,Address,CSM,BirthDate,BirthPlace,WeightOnBirth,DiagnosisAge,SpasticId,DiscinesisId,AtacsiumId,ShortAnamnez,EpilepcyId,EpilepcyType1,EpilepcyType2,EpilepcyType3,EpilepcyType4,EpilepcyType5,IcpPeriodId,IcpReasonId,Diagnosis")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AtacsiumId = new SelectList(db.Atacsiums, "Id", "Name", patient.AtacsiumId);
            ViewBag.DiscinesisId = new SelectList(db.Discinesis, "Id", "Name", patient.DiscinesisId);
            ViewBag.EpilepcyId = new SelectList(db.Epilepcies, "Id", "Name", patient.EpilepcyId);
            ViewBag.IcpPeriodId = new SelectList(db.IcpPeriods, "Id", "Name", patient.IcpPeriodId);
            ViewBag.IcpReasonId = new SelectList(db.IcpReasons, "Id", "Name", patient.IcpReasonId);
            ViewBag.SpasticId = new SelectList(db.Spastics, "Id", "Name", patient.SpasticId);
            return View(patient);
        }

        // GET: Patients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewBag.AtacsiumId = new SelectList(db.Atacsiums, "Id", "Name", patient.AtacsiumId);
            ViewBag.DiscinesisId = new SelectList(db.Discinesis, "Id", "Name", patient.DiscinesisId);
            ViewBag.EpilepcyId = new SelectList(db.Epilepcies, "Id", "Name", patient.EpilepcyId);
            ViewBag.IcpPeriodId = new SelectList(db.IcpPeriods, "Id", "Name", patient.IcpPeriodId);
            ViewBag.IcpReasonId = new SelectList(db.IcpReasons, "Id", "Name", patient.IcpReasonId);
            ViewBag.SpasticId = new SelectList(db.Spastics, "Id", "Name", patient.SpasticId);
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LastName,FirstName,MiddleName,Contacts,Address,CSM,BirthDate,BirthPlace,WeightOnBirth,DiagnosisAge,SpasticId,DiscinesisId,AtacsiumId,ShortAnamnez,EpilepcyId,EpilepcyType1,EpilepcyType2,EpilepcyType3,EpilepcyType4,EpilepcyType5,IcpPeriodId,IcpReasonId,Diagnosis")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AtacsiumId = new SelectList(db.Atacsiums, "Id", "Name", patient.AtacsiumId);
            ViewBag.DiscinesisId = new SelectList(db.Discinesis, "Id", "Name", patient.DiscinesisId);
            ViewBag.EpilepcyId = new SelectList(db.Epilepcies, "Id", "Name", patient.EpilepcyId);
            ViewBag.IcpPeriodId = new SelectList(db.IcpPeriods, "Id", "Name", patient.IcpPeriodId);
            ViewBag.IcpReasonId = new SelectList(db.IcpReasons, "Id", "Name", patient.IcpReasonId);
            ViewBag.SpasticId = new SelectList(db.Spastics, "Id", "Name", patient.SpasticId);
            return View(patient);
        }

        // GET: Patients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
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
