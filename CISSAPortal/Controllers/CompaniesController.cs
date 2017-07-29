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
    public class CompaniesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Companies
        public ActionResult Index()
        {
            var companies = db.Companies.Include(c => c.AspNetUser).ToList();
            var RGUSOrgId = new Guid("{6853C82D-751E-40DD-AA14-21AF0AB7C64E}");
            var cissameta = new CissaMeta.MetaProxy();
            var usrList = cissameta.GetUSRList();

            ViewBag.UsrList = usrList.Where(x => companies.Select(c => c.OrgId ?? Guid.Empty).Contains(x.Id));

            return View(companies);
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create(string userId, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            var RGUSOrgId = new Guid("{6853C82D-751E-40DD-AA14-21AF0AB7C64E}");
            var cissameta = new CissaMeta.MetaProxy();
            var usrList = cissameta.GetUSRList();
            //ViewBag.AspNetUserId = new SelectList(db.Users, "Id", "Email");

            ViewBag.OrgId = new SelectList(usrList, "Id", "Name");
            var model = new Company { AspNetUserId = userId };
            return View(model);
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Address,AspNetUserId,INN,OKPO,ActivityType,Telephone,BankName,BIK,BankAccountNo,CompanyAccountNo,OrgId")] Company company, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                db.Companies.Add(company);
                db.SaveChanges();
                if (returnUrl != null)
                    return Redirect(returnUrl);
                return RedirectToAction("Index");
            }

            ViewBag.AspNetUserId = new SelectList(db.Users, "Id", "Email", company.AspNetUserId);
            return View(company);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }

            var RGUSOrgId = new Guid("{6853C82D-751E-40DD-AA14-21AF0AB7C64E}");
            var cissameta = new CissaMeta.MetaProxy();
            var usrList = cissameta.GetUSRList();
            //ViewBag.AspNetUserId = new SelectList(db.Users, "Id", "Email");

            ViewBag.OrgId = new SelectList(usrList, "Id", "Name", company.OrgId);

            ViewBag.AspNetUserId = new SelectList(db.Users, "Id", "Email", company.AspNetUserId);
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address,AspNetUserId,INN,OKPO,ActivityType,Telephone,BankName,BIK,BankAccountNo,CompanyAccountNo,OrgId")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AspNetUserId = new SelectList(db.Users, "Id", "Email", company.AspNetUserId);
            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
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
