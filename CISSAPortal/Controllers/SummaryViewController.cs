using CISSAPortal.Models;
using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CISSAPortal.Controllers
{
    public class SummaryViewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: SummaryView
        public ActionResult Index(MonitorViewModel model)
        {
            ViewBag.Regions = db.HumDistributionPlanItems.Select(x => x.Region).Distinct().ToList();
            var companies = db.Companies.Select(x => new { x.Id, x.Name }).ToList();
            companies.Insert(0, new { Id = -1, Name = "-" });
            ViewBag.CompanyId = new SelectList(companies, "Id", "Name");

            ViewBag.Quarters = db.Reports.Select(x => x.Quarter.Value).Distinct().OrderBy(x => x).ToList();

            ViewBag.Years = db.Reports.Select(x => x.Year.Value).Distinct().OrderBy(x => x).ToList();

            DateTime fd = new DateTime();
            DateTime ld = new DateTime();
            if (model.Quarter.HasValue && model.Year.HasValue)
            {
                fd = GetBeginDateOfQuarter(model.Year.Value, model.Quarter.Value);
                ld = GetFinishDateOfQuarter(model.Year.Value, model.Quarter.Value);
            }

            var plans = new List<HumDistributionPlanItem>();
            if(!string.IsNullOrEmpty(model.Region))
                plans = db.HumDistributionPlanItems.Where(x => x.Region == model.Region).ToList();
            var reports = db.Reports.ToList();
            var dbcompanies = db.Companies.ToList();

            var regions = new List<RegionViewModel>();
            foreach(var plan in plans)
            {

            }

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