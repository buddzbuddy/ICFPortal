using IdentitySample.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace CISSAPortal.Controllers
{
    public static class DocumentExtensions
    {
        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }
    }

    public class CurrencyRates
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public List<Currency> List { get; set; }
    }

    public class Currency
    {
        public string ISOCode { get; set; }
        public int? Nominal { get; set; }
        public decimal? Value { get; set; }
    }
    public class ExchangeController : Controller
    {
        // GET: Exchange
        public ActionResult Index()
        {
            return View(GetCurrencyRates());
        }

        public JsonResult SetCurrencyRate(int planId, string rateValueString)
        {
            var db = new ApplicationDbContext();
            db.Database.BeginTransaction();
            try
            {
                var planObj = db.HumDistributionPlans.Find(planId);
                decimal rateValue;
                if (!(decimal.TryParse(rateValueString, out rateValue) || decimal.TryParse(rateValueString.Replace('.', ','), out rateValue)))
                    throw new Exception("Can't convert to decimal: " + rateValueString);
                planObj.CurrencyRate = rateValue;
                planObj.CurrencyRateDate = DateTime.Now;
                db.Entry(planObj).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                db.Database.CurrentTransaction.Commit();
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                db.Database.CurrentTransaction.Rollback();
                return Json(new { result = false, errorMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCurrency(int planId)
        {
            var db = new ApplicationDbContext();
            var planObj = db.HumDistributionPlans.Find(planId);

            var rates = GetCurrencyRates();
            var currency = rates.List.FirstOrDefault(x => x.ISOCode == planObj.CurrencyISOCode);
            return Json(currency, JsonRequestBehavior.AllowGet);
        }
        
        public CurrencyRates GetCurrencyRates()
        {
            var xmldoc = new XmlDocument();

            string urlOrigin = "http://www.nbkr.kg/XML/daily.xml";
            string urlFake = Server.MapPath("~/Doc/daily.xml");
            string url = urlFake;
            if (ConfigurationManager.AppSettings["HasInternetConnection"] == "yes")
                url = urlOrigin;
            xmldoc.Load(url);

            var xdoc = DocumentExtensions.ToXDocument(xmldoc);
            var CurrencyRates = new CurrencyRates
            {
                Name = xdoc.Root.Attribute("Name").Value.ToString(),
                Date = xdoc.Root.Attribute("Date").Value.ToString()
            };
            CurrencyRates.List = new List<Currency>();
            foreach (var rate in xdoc.Root.Elements())
            {
                var isoCode = rate.Attribute("ISOCode").Value;
                var nominal = rate.Element("Nominal").Value;
                var value = rate.Element("Value").Value.Replace(',', '.');
                var currency = new Currency();
                currency.ISOCode = isoCode;
                int nominalInt;
                if (int.TryParse(nominal, out nominalInt))
                    currency.Nominal = nominalInt;
                else throw new Exception("Can't convert to int: " + nominal);
                decimal valueDec;
                if (decimal.TryParse(value, out valueDec))
                    currency.Value = valueDec;
                else if (decimal.TryParse(value.Replace('.', ','), out valueDec))
                    currency.Value = valueDec;
                else throw new Exception("Can't convert to decimal: " + value);
                CurrencyRates.List.Add(currency);
            }
            return CurrencyRates;
        }
    }
}
