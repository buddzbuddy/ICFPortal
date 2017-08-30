using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace CISSAPortal.Controllers
{
    public class HumDistributionPlansController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HumDistributionPlans
        public ActionResult Index()
        {
            var humDistributionPlans = db.HumDistributionPlans.Include(h => h.Company).Where(x => x.Company.AspNetUser.UserName == User.Identity.Name);
            return View(humDistributionPlans.ToList());
        }

        // GET: HumDistributionPlans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HumDistributionPlan humDistributionPlan = db.HumDistributionPlans.Find(id);
            if (humDistributionPlan == null)
            {
                return HttpNotFound();
            }
            return View(humDistributionPlan);
        }

        // GET: HumDistributionPlans/Create
        public async Task<ActionResult> Create()
        {
            var uManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var userInfo = await uManager.FindByNameAsync(User.Identity.Name);
            //ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name");
            var model = new HumDistributionPlan();
            if(userInfo.Companies.Count > 0)
            {
                model.CompanyId = userInfo.Companies.First().Id;
                model.Company = userInfo.Companies.First();
            }
            else
            {
                return RedirectToAction("Create", "Companies", new { userId = userInfo.Id, returnUrl = Request.Path });
            }
            return View(model);
        }

        // POST: HumDistributionPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CompanyId,Date")] HumDistributionPlan humDistributionPlan)
        {
            if (ModelState.IsValid)
            {
                db.HumDistributionPlans.Add(humDistributionPlan);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = humDistributionPlan.Id });
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", humDistributionPlan.CompanyId);
            return View(humDistributionPlan);
        }

        // GET: HumDistributionPlans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HumDistributionPlan humDistributionPlan = db.HumDistributionPlans.Find(id);
            if (humDistributionPlan == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", humDistributionPlan.CompanyId);
            return View(humDistributionPlan);
        }

        // POST: HumDistributionPlans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyId,Date")] HumDistributionPlan humDistributionPlan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(humDistributionPlan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", humDistributionPlan.CompanyId);
            return View(humDistributionPlan);
        }

        // GET: HumDistributionPlans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HumDistributionPlan humDistributionPlan = db.HumDistributionPlans.Find(id);
            if (humDistributionPlan == null)
            {
                return HttpNotFound();
            }
            return View(humDistributionPlan);
        }

        // POST: HumDistributionPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HumDistributionPlan humDistributionPlan = db.HumDistributionPlans.Find(id);
            db.HumDistributionPlans.Remove(humDistributionPlan);
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
