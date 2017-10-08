using CISSAPortal.Models;
using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace CISSAPortal.Controllers
{
    public class DiagramController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Diagram
        public ActionResult Index(DiagramViewModel model)
        {
            var areas = db.Areas.ToList();
            areas.Insert(0, new Area());
            ViewBag.AreaId = new SelectList(areas, "Id", "Name", model.AreaId);

            if(model.EntityType == EntityType.Produt)
            {
                QueryProducts(model);
                ViewBag.QueryTitle = "Распределено гум. помощи";
            }
            else if(model.EntityType == EntityType.Certificate)
            {
                QueryCertificates(model);
                ViewBag.QueryTitle = "Выдано заключений";
            }

            return View(model);
        }

        void QueryProducts(DiagramViewModel model)
        {

            var planItems = new List<HumDistributionPlanItem>();
            var planItemsQuery = db.HumDistributionPlanItems as IQueryable<HumDistributionPlanItem>;
            /*if (model.AreaId != 0)
            {
                planItems = (from pi in db.HumDistributionPlanItems
                             where pi.AreaId == model.AreaId
                             select pi).ToList();
                planItemsQuery = planItemsQuery.Where(x => x.AreaId == model.AreaId);
            }
            else
                planItems = db.HumDistributionPlanItems.ToList();*/
            if (model.AreaId != 0)
            {
                planItemsQuery = planItemsQuery.Where(x => x.AreaId == model.AreaId);
            }
            if (model.From != null)
            {
                planItemsQuery = planItemsQuery.Where(x => x.HumDistributionPlan.Date >= model.From);
            }
            if (model.To != null)
            {
                planItemsQuery = planItemsQuery.Where(x => x.HumDistributionPlan.Date <= model.To);
            }
            planItems = planItemsQuery.ToList();

            var planItemIds = planItems.Select(x => x.Id).ToList();
            var reportItems = db.ReportItems.Where(x => planItemIds.Contains(x.HumDistributionPlanItemId ?? 0)).ToList();
            foreach (var productItems in planItems.GroupBy(x => x.ProductId))
            {
                var productName = db.Products.Find(productItems.Key).Name;
                foreach (var unitTypeItems in productItems.GroupBy(x => x.UnitTypeId))
                {
                    var unitTypeName = db.UnitTypes.Find(unitTypeItems.Key).Name;
                    var modelItem = new BarDiagramItem
                    {
                        CategoryName = string.Format("{0} ({1})", productName, unitTypeName.ToLower())
                    };

                    modelItem.Received = (int)unitTypeItems.Sum(x => x.Amount ?? 0.0);
                    var unitTypeItemIds = unitTypeItems.Select(x => x.Id);

                    var unitReportItems = reportItems.Where(x => unitTypeItemIds.Contains(x.HumDistributionPlanItemId ?? 0));
                    if (unitReportItems != null && unitReportItems.Count() > 0)
                        modelItem.Issued = (int)unitReportItems.Sum(x => x.FactAmount);

                    model.BarDiagramItems.Add(modelItem);

                    //Pie items
                    model.PieDiagramItems.Add(new PieDiagramItem
                    {
                        Name = modelItem.CategoryName,
                        Amount = modelItem.Received
                    });
                }
            }
            model.BarDiagramItems = model.BarDiagramItems.OrderBy(x => x.CategoryName).ToList();
        }

        void QueryCertificates(DiagramViewModel model)
        {

            var planItems = new List<HumDistributionPlanItem>();

            if (model.AreaId != 0)
            {
                planItems = db.HumDistributionPlanItems.Where(pi => pi.AreaId == model.AreaId).Include(x => x.HumDistributionPlan).ToList();
            }
            else
                planItems = db.HumDistributionPlanItems.ToList();

            var planItemIds = planItems.Select(x => x.Id).ToList();
            var reportItems = db.ReportItems.Where(x => planItemIds.Contains(x.HumDistributionPlanItemId ?? 0)).ToList();
            foreach (var productItems in planItems.GroupBy(x => x.ProductId))
            {
                var productName = db.Products.Find(productItems.Key).Name;
                foreach (var unitTypeItems in productItems.GroupBy(x => x.UnitTypeId))
                {
                    var unitTypeName = db.UnitTypes.Find(unitTypeItems.Key).Name;
                    var modelItem = new BarDiagramItem
                    {
                        CategoryName = string.Format("{0} ({1})", productName, unitTypeName.ToLower())
                    };

                    var certificates = unitTypeItems.Select(x => x.HumDistributionPlan.CertificateNo ?? "").Distinct().Count();
                    modelItem.Received = certificates;
                    modelItem.Balance = 0;
                    model.BarDiagramItems.Add(modelItem);

                    //Pie items
                    model.PieDiagramItems.Add(new PieDiagramItem
                    {
                        Name = modelItem.CategoryName,
                        Amount = modelItem.Received
                    });
                }
            }
            model.BarDiagramItems = model.BarDiagramItems.OrderBy(x => x.CategoryName).ToList();
        }
    }
}