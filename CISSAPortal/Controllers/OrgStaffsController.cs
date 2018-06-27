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
    public class OrgStaffsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: OrgStaffs
        public ActionResult Index()
        {
            var orgStaffs = db.OrgStaffs.Include(o => o.ServiceOrganization);
            return View(orgStaffs.ToList());
        }

        public ActionResult GetStaffsByOrgIds(string orgs)
        {
            if (!string.IsNullOrEmpty(orgs))
            {
                var typeIds = orgs.Split(',').Select(x => int.Parse(x));
                var staffs = db.OrgStaffs.Where(x => typeIds.Contains(x.ServiceOrganizationId.Value)).ToList();
                if (staffs.Count > 0)
                    return Json(new { result = true, items = staffs.Select(x => new { x.Id, x.Name }) }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { result = false, errorMessage = "Ни одна должность не найдена по выбранным организациям" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { result = false, errorMessage = "Организация не выбрана" }, JsonRequestBehavior.AllowGet);
        }

        // GET: OrgStaffs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrgStaff orgStaff = db.OrgStaffs.Find(id);
            if (orgStaff == null)
            {
                return HttpNotFound();
            }
            return View(orgStaff);
        }

        // GET: OrgStaffs/Create
        public ActionResult Create(int ServiceOrganizationId)
        {
            ViewBag.ServiceOrganizationId = new SelectList(db.ServiceOrganizations, "Id", "Name", ServiceOrganizationId);
            return View(new OrgStaff { ServiceOrganizationId = ServiceOrganizationId });
        }

        // POST: OrgStaffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ServiceOrganizationId")] OrgStaff orgStaff)
        {
            if (ModelState.IsValid)
            {
                db.OrgStaffs.Add(orgStaff);
                db.SaveChanges();
                return RedirectToAction("Details", "ServiceOrganizations", new { id = orgStaff.ServiceOrganizationId });
            }

            ViewBag.ServiceOrganizationId = new SelectList(db.ServiceOrganizations, "Id", "Name", orgStaff.ServiceOrganizationId);
            return View(orgStaff);
        }

        // GET: OrgStaffs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrgStaff orgStaff = db.OrgStaffs.Find(id);
            if (orgStaff == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServiceOrganizationId = new SelectList(db.ServiceOrganizations, "Id", "Name", orgStaff.ServiceOrganizationId);
            return View(orgStaff);
        }

        // POST: OrgStaffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ServiceOrganizationId")] OrgStaff orgStaff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orgStaff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ServiceOrganizationId = new SelectList(db.ServiceOrganizations, "Id", "Name", orgStaff.ServiceOrganizationId);
            return View(orgStaff);
        }

        // GET: OrgStaffs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrgStaff orgStaff = db.OrgStaffs.Find(id);
            if (orgStaff == null)
            {
                return HttpNotFound();
            }
            return View(orgStaff);
        }

        // POST: OrgStaffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrgStaff orgStaff = db.OrgStaffs.Find(id);
            db.OrgStaffs.Remove(orgStaff);
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
