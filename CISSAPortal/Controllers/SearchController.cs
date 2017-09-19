using CISSAPortal.Models;
using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CISSAPortal.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Search
        public ActionResult Index(SearchViewModel model)
        {
            ViewBag.Regions = db.HumDistributionPlanItems.Select(x => x.Region).Distinct().ToList();
            var companies = db.Companies.Select(x => new { x.Id, x.Name }).ToList();
            companies.Insert(0, new { Id = -1, Name = "-" });
            ViewBag.CompanyId = new SelectList(companies, "Id", "Name");

            ViewBag.Quarters = db.Reports.Select(x => x.Quarter.Value).Distinct().OrderBy(x => x).ToList();

            ViewBag.Years = db.Reports.Select(x => x.Year.Value).Distinct().OrderBy(x => x).ToList();

            DateTime fd = new DateTime();
            DateTime ld = new DateTime();
            if(model.Quarter.HasValue && model.Year.HasValue)
            {
                fd = GetBeginDateOfQuarter(model.Year.Value, model.Quarter.Value);
                ld = GetFinishDateOfQuarter(model.Year.Value, model.Quarter.Value);
            }

            model.HumDistributionPlans = (from p in db.HumDistributionPlans
                                         where
                                         (model.CompanyId != -1 ? p.Company.Id == model.CompanyId : true)
                                         &&
                                         (!string.IsNullOrEmpty(model.Region) ? p.HumDistributionPlanItems.Any(x => x.Region == model.Region) : true)
                                         &&
                                         (model.Year.HasValue && model.Quarter.HasValue ? (p.Date >= fd && p.Date <= ld) : true)
                                          select p).ToList();
            model.Reports = (from r in db.Reports
                             where
                             (model.CompanyId != -1 ? r.HumDistributionPlan.Company.Id == model.CompanyId : true)
                             &&
                             (model.Year.HasValue ? (r.Year == model.Year) : true)
                             &&
                             (model.Quarter.HasValue ? (r.Quarter == model.Quarter) : true)
                             &&
                             (!string.IsNullOrEmpty(model.Region) ? r.HumDistributionPlan.HumDistributionPlanItems.Any(x => x.Region == model.Region) : true)
                             select r).ToList();
            return View(model);
        }

        private DateTime GetBeginDateOfQuarter(int year, int quarter)
        {
            return new DateTime(year, (quarter == 1 ? 1 : quarter == 2 ? 4 : quarter == 3 ? 7 : 10), 1);
        }

        private DateTime GetFinishDateOfQuarter(int year, int quarter)
        {
            var m = (quarter == 1 ? 3 : quarter == 2 ? 6 : quarter == 3 ? 9 : 12);
            return new DateTime(year, m, DateTime.DaysInMonth(year, m));
        }
    }
}