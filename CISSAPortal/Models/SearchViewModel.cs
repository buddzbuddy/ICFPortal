using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CISSAPortal.Models
{
    public class SearchViewModel
    {
        [Display(Name = "Регион")]
        public int? AreaId { get; set; }
        [Display(Name = "Компания")]
        public int? CompanyId { get; set; }
        [Display(Name = "Квартал"), Range(1, 4)]
        public int? Quarter { get; set; }
        [Display(Name = "Год"), Range(2017, 2099)]
        public int? Year { get; set; }


        public List<HumDistributionPlan> HumDistributionPlans { get; set; }
        public List<Report> Reports { get; set; }
    }
}