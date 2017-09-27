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

            var planItems = new List<HumDistributionPlanItem>();
            var reportItems = new List<ReportItem>();
            /*if(!string.IsNullOrEmpty(model.Region))
            {
                planItems = db.HumDistributionPlanItems.Where(x => x.Region == model.Region).ToList();
                var planItemIds = planItems.Select(p => p.Id).ToList();
                reportItems = db.ReportItems.Where(x => planItemIds.Contains(x.HumDistributionPlanItemId ?? 0)).ToList();
            }*/

            if (model.CompanyId.HasValue && model.CompanyId != -1)
            {
                planItems = db.HumDistributionPlanItems.Where(x => x.HumDistributionPlan.CompanyId == model.CompanyId).Include(x => x.HumDistributionPlan).ToList();
                var planItemIds = planItems.Select(p => p.Id).ToList();
                reportItems = db.ReportItems.Where(x => planItemIds.Contains(x.HumDistributionPlanItemId ?? 0)).ToList();
            }
            else
            {
                planItems = db.HumDistributionPlanItems.Include(x => x.HumDistributionPlan).ToList();
                reportItems = db.ReportItems.ToList();
            }

            //var dbcompanies = db.Companies.ToList();

            var regions = new List<RegionViewModel>();
            foreach(var planItems2 in planItems.GroupBy(x => x.Region))
            {
                var region = new RegionViewModel();
                region.Name = planItems2.Key;
                region.PlanSum = planItems2.Sum(x => x.Sum ?? 0);

                var planItemIds2 = planItems2.Select(p => p.Id).ToList();
                var reportItems2 = reportItems.Where(x => planItemIds2.Contains(x.HumDistributionPlanItemId ?? 0));
                if(reportItems2 != null)
                region.FactSum = reportItems2.Sum(x => x.FactSum ?? 0);

                region.PlanItems = planItems2.Select(x => x.Id).ToList();

                regions.Add(region);
            }
            model.Regions = regions;
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

        [HttpPost]
        public ActionResult ViewByRegion(FormCollection collection)
        {
            var regionPlanItemIds = collection["planItems"].Split(',').Select(x => int.Parse(x)).ToList();
            var regionPlanItems = db.HumDistributionPlanItems.Where(x => regionPlanItemIds.Contains(x.Id)).Include(x => x.HumDistributionPlan).ToList();
            if(regionPlanItems.Count == 0)
            {
                throw new Exception("PlanItems not found in database. src:" + collection["planItems"]);
            }
            var model = new MonitorViewModel();
            model.Region = collection["regionName"];
            var companies = new List<CompanyViewModel>();
            foreach(var companyPlanItems in regionPlanItems.GroupBy(x => x.HumDistributionPlan.CompanyId ?? 0))
            {
                var company = new CompanyViewModel();
                company.Id = companyPlanItems.Key;
                company.Name = db.Companies.Find(companyPlanItems.Key).Name;
                company.PlanSum = companyPlanItems.Sum(x => x.Sum ?? 0);
                var companyPlanItemIds = companyPlanItems.Select(p => p.Id).ToList();
                var companyReportItems = db.ReportItems.Where(x => companyPlanItemIds.Contains(x.HumDistributionPlanItemId ?? 0)).ToList();
                company.FactSum = companyReportItems.Sum(x => x.FactSum ?? 0);
                
                company.PlanItems = companyPlanItemIds;

                companies.Add(company);
            }
             model.Companies = companies;
            return View(model);
        }

        [HttpPost]
        public ActionResult ViewByCompany(FormCollection collection)
        {
            var companyPlanItemIds = collection["planItems"].Split(',').Select(x => int.Parse(x)).ToList();
            ViewBag.CompanyName = collection["companyName"];
            ViewBag.CompanyId = collection["companyId"];
            ViewBag.RegionName = collection["regionName"];
            var companyPlanItems = db.HumDistributionPlanItems.Where(x => companyPlanItemIds.Contains(x.Id)).ToList();
            if (companyPlanItems == null)
            {
                throw new Exception("PlanItems not found in database. src:" + collection["planItems"]);
            }
            return View(companyPlanItems);
        }

        public ActionResult ViewReportSummaryByYears(int companyId)
        {
            var model = new CompanyByYearsViewModel { CompanyId = companyId, Years = new List<YearViewModel>() };
            model.Company = db.Companies.Find(companyId);

            var companyItems = db.ReportItems.Where(x => x.Report.HumDistributionPlan.CompanyId == companyId).Include(x => x.HumDistributionPlanItem).ToList();

            if(companyItems.Count > 0)
            {
                foreach(var yearItems in companyItems.GroupBy(x => x.Report.Year))
                {
                    var yearModel = new YearViewModel { Year = yearItems.Key ?? 0, Quarters = new List<QuarterViewModel>() };
                    foreach(var quarterItems in yearItems.GroupBy(x => x.Report.Quarter ?? 0))
                    {
                        var quarterModel = new QuarterViewModel { Quarter = quarterItems.Key };
                        quarterModel.FactSum = quarterItems.Sum(x => x.FactSum ?? 0);
                        quarterModel.PlanSum = quarterItems.Sum(x => x.HumDistributionPlanItem.Sum ?? 0);
                        yearModel.Quarters.Add(quarterModel);
                    }
                    model.Years.Add(yearModel);
                }
            }
            return View(model);
        }
    }
}