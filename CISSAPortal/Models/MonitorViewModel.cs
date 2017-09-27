using IdentitySample.Models;
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
        public string Name { get; set; }
    }

    public class CompanyViewModel: MonitorItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CompanyByYearsViewModel
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public List<YearViewModel> Years { get; set; }
    }
    public class YearViewModel
    {
        [Display(Name = "Год"), Range(2017, 2099)]
        public int Year { get; set; }
        public List<QuarterViewModel> Quarters { get; set; }
    }
    public class QuarterViewModel: MonitorItemViewModel
    {
        [Display(Name = "Квартал"), Range(1, 4)]
        public int Quarter { get; set; }
    }


    public abstract class MonitorItemViewModel
    {
        [Display(Name = "План")]
        public decimal PlanSum { get; set; }
        [Display(Name = "Факт.")]
        public decimal FactSum { get; set; }
        [Display(Name = "Остаток")]
        public decimal Balance
        {
            get
            {
                return PlanSum - FactSum;
            }
        }
        public List<int> PlanItems { get; set; }
    }
}