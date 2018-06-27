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
    public class ServiceOrganizationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ServiceOrganizations
        public ActionResult Index()
        {
            return View(db.ServiceOrganizations.Include(x => x.ServiceTypes).Include(x => x.OrgStaffs).ToList());
        }

        public ActionResult GetByServiceTypes(string types)
        {
            if (!string.IsNullOrEmpty(types))
            {
                var typeIds = types.Split(',').Select(x => int.Parse(x));
                var orgEqC = new OrgEqualityComparer();
                var orgs = db.ServiceTypes.Where(x => x.ServiceOrganizationId.HasValue && typeIds.Contains(x.Id)).Select(x => x.ServiceOrganization).ToList();
                if (orgs.Count > 0)
                    return Json(new { result = true, items = orgs.Distinct(orgEqC).Select(x => new { x.Id, x.Name }) }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { result = false, errorMessage = "Ни одна организация не найдена по выбранным услугам" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { result = false, errorMessage = "Услуга не выбрана" }, JsonRequestBehavior.AllowGet);
        }
        class OrgEqualityComparer : IEqualityComparer<ServiceOrganization>
        {
            public bool Equals(ServiceOrganization b1, ServiceOrganization b2)
            {
                if (b2 == null && b1 == null)
                    return true;
                else if (b1 == null | b2 == null)
                    return false;
                else if (b1.Name == b2.Name && b1.Id == b2.Id)
                    return true;
                else
                    return false;
            }

            public int GetHashCode(ServiceOrganization obj)
            {
                return obj.GetHashCode();
            }
        }
        // GET: ServiceOrganizations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceOrganization serviceOrganization = db.ServiceOrganizations.Include(x => x.ServiceTypes).Include(x => x.ServiceTypes).FirstOrDefault(x => x.Id == id);
            if (serviceOrganization == null)
            {
                return HttpNotFound();
            }
            return View(serviceOrganization);
        }

        // GET: ServiceOrganizations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServiceOrganizations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ServiceTypes,DisabilityTypes,Ages,BeneficiaryAverage,Address,ManagerName,StaffPositions,StaffPositionsCount,FinanceSource")] ServiceOrganization serviceOrganization)
        {
            if (ModelState.IsValid)
            {
                db.ServiceOrganizations.Add(serviceOrganization);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(serviceOrganization);
        }

        // GET: ServiceOrganizations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceOrganization serviceOrganization = db.ServiceOrganizations.Find(id);
            if (serviceOrganization == null)
            {
                return HttpNotFound();
            }
            return View(serviceOrganization);
        }

        // POST: ServiceOrganizations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ServiceTypes,DisabilityTypes,Ages,BeneficiaryAverage,Address,ManagerName,StaffPositions,StaffPositionsCount,FinanceSource")] ServiceOrganization serviceOrganization)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceOrganization).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(serviceOrganization);
        }

        // GET: ServiceOrganizations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceOrganization serviceOrganization = db.ServiceOrganizations.Find(id);
            if (serviceOrganization == null)
            {
                return HttpNotFound();
            }
            return View(serviceOrganization);
        }

        // POST: ServiceOrganizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServiceOrganization serviceOrganization = db.ServiceOrganizations.Find(id);
            db.ServiceOrganizations.Remove(serviceOrganization);
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
