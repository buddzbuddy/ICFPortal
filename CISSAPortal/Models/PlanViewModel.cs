using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CISSAPortal.Models
{
    public class PlanViewModel
    {

        [Required]
        [ForeignKey("Company")]
        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [Display(Name = "Дата предоставления плана")]
        [Required]
        public DateTime? Date { get; set; }
        /// <summary>
        /// Foreign Key to State
        /// </summary>
        [ForeignKey("State")]
        public int? StateId { get; set; }
        public virtual DocumentState State { get; set; }

        [Required]
        [MaxLength(3), MinLength(3)]
        [Display(Name = "Валюта")]
        public string CurrencyISOCode { get; set; }

        [Display(Name = "Курс в сомах")]
        public decimal? CurrencyRate { get; set; }

        [Display(Name = "Дата курса")]
        public DateTime? CurrencyRateDate { get; set; }

        [Display(Name = "Дата выдачи заключения")]
        public DateTime? CertificateDate { get; set; }

        [Display(Name = "№ заключения")]
        public string CertificateNo { get; set; }

        public IList<PlanItemViewModel> Items { get; set; }
    }
    public class PlanItemViewModel
    {
        [Required]
        [Display(Name = "Потребитель / Организация")]
        public string Consumer { get; set; }

        [Required]
        [Display(Name = "Регион")]
        public int? AreaId { get; set; }
        public virtual Area Area { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Наименование гум. помощи (товара)")]
        public string ProductName { get; set; }

        [Display(Name = "Ед. изм.")]
        [ForeignKey("UnitType")]
        public int UnitTypeId { get; set; }
        public virtual UnitType UnitType { get; set; }

        [Required]
        [Display(Name = "Кол-во")]
        public int? Amount { get; set; }

        //[Required]
        [Display(Name = "Вес")]
        public double? Weight { get; set; }

        [Required]
        [Display(Name = "Сумма")]
        public decimal? Sum { get; set; }
    }
}