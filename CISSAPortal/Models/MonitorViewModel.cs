using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CISSAPortal.Models
{
    public class MonitorViewModel
    {
        [Display(Name = "Регион")]
        public string Region { get; set; }
        [Display(Name = "Компания")]
        public int? CompanyId { get; set; }
        [Display(Name = "Квартал"), Range(1, 4)]
        public int? Quarter { get; set; }
        [Display(Name = "Год"), Range(2017, 2099)]
        public int? Year { get; set; }

        public List<RegionViewModel> Regions { get; set; }

        public List<CompanyViewModel> Companies { get; set; }
    }

    public class RegionViewModel: MonitorItemViewModel
    {
        public string Region { get; set; }
    }

    public class CompanyViewModel: MonitorItemViewModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }

    public abstract class MonitorItemViewModel
    {
        public decimal PlanSum { get; set; }
        public decimal FactSum { get; set; }
        public decimal Balance
        {
            get
            {
                return PlanSum - FactSum;
            }
        }
    }
}