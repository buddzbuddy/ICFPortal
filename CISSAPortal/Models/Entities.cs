using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentitySample.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }

    public class Company
    {
        /// <summary>
        /// Key
        /// </summary>
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название организации")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Юридический адрес")]
        public string Address { get; set; }

        /// <summary>
        /// Foreign Key
        /// </summary>
        [StringLength(128), MinLength(3)]
        [ForeignKey("AspNetUser")]
        public string AspNetUserId { get; set; }
        public virtual ApplicationUser AspNetUser { get; set; }

        [Display(Name = "ИНН")]
        public string INN { get; set; }

        [Display(Name = "Код ОКПО")]
        public string OKPO { get; set; }

        [Display(Name = "Вид деятельности")]
        public string ActivityType { get; set; }

        [Display(Name = "Телефон")]
        public string Telephone { get; set; }

        [Display(Name = "Эл. почта")]
        public virtual string Email { get { return AspNetUser != null ? AspNetUser.Email : ""; } }

        [Display(Name = "Банк")]
        public string BankName { get; set; }

        [Display(Name = "БИК")]
        public string BIK { get; set; }

        [Display(Name = "Р/счет")]
        public string BankAccountNo { get; set; }

        [Display(Name = "Л/счет компании")]
        public string CompanyAccountNo { get; set; }

        [Display(Name = "Управление социального развития")]
        public Guid? OrgId { get; set; }
    }

    public class Report
    {
        [Key]
        public int Id { get; set; }
        [Required, Range(2017, 2099)]
        [Display(Name ="Год")]
        public int? Year { get; set; }
        [Required, Range(1, 4)]
        [Display(Name = "Квартал")]
        public int? Quarter { get; set; }
        [Required]
        [Display(Name = "Дата")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? Date { get; set; }
        /// <summary>
        /// Foreign Key
        /// </summary>
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// Foreign Key to State
        /// </summary>
        [ForeignKey("State")]
        public int? StateId { get; set; }
        public virtual DocumentState State { get; set; }

        [ForeignKey("HumDistributionPlan")]
        public int? HumDistributionPlanId { get; set; }
        public virtual HumDistributionPlan HumDistributionPlan { get; set; }

        public virtual ICollection<ReportItem> ReportItems { get; set; }
    }

    public class ReportItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Report")]
        public int? ReportId { get; set; }
        public virtual Report Report { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:# ### ### ##0.0}")]
        [Display(Name = "Кол-во")]
        public double? FactAmount { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:# ### ### ##0.0}")]
        [Display(Name = "Сумма")]
        public decimal? FactSum { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:# ### ### ##0.0}")]
        [Display(Name = "Кол-во")]
        public double? SpoiledAmount { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:# ### ### ##0.0}")]
        [Display(Name = "Сумма")]
        public decimal? SpoiledSum { get; set; }

        [Display(Name = "Кол-во")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:# ### ### ##0.0}")]
        public double? BalanceAmount { get; set; }
        [Display(Name = "Сумма")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:# ### ### ##0.0}")]
        public decimal? BalanceSum { get; set; }

        [ForeignKey("HumDistributionPlanItem")]
        public int? HumDistributionPlanItemId { get; set; }
        public HumDistributionPlanItem HumDistributionPlanItem { get; set; }
    }

    public class UnitType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ед. измерения")]
        public string Name { get; set; }

        public Guid? EnumId { get; set; }
    }

    public class DocumentState
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Статус")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Код")]
        public int Code { get; set; }
    }

    public class LegalReportSection
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [Display(Name = "Дата предоставления отчета")]
        [Required]
        public DateTime? ReportDate { get; set; }
        [Display(Name = "1. Остаток задолжности УСР на начало года")]
        public decimal? BalanceBegin { get; set; }
        [Display(Name = "2. Всего начислено пособий за счет республиканского бюджета с начала года")]
        public decimal? AccruedBenefitsBegin { get; set; }
        [Display(Name = "2а. в том числе за месяц")]
        public decimal? IncludingMonth { get; set; }
        [Display(Name = "3. Увеличение (+), уменьшение (-) суммы пособий по результатам проверки")]
        public decimal? IncreaseDecreaseAmount { get; set; }
        [Display(Name = "4. Возмещено республиканским бюджетом начала года")]
        public decimal? RefundedRepBudgetBegin { get; set; }
        [Display(Name = "5. Остаток задолженности на конец отчетного месяца")]
        public decimal? DebtBalanceEnd { get; set; }
        /// <summary>
        /// Foreign Key to State
        /// </summary>
        [ForeignKey("State")]
        public int? StateId { get; set; }
        public virtual DocumentState State { get; set; }

        public virtual ICollection<BirthInfoOnPayBenefit> BirthInfoOnPayBenefits { get; set; }
        public virtual ICollection<DeadInfoOnPayBenefit> DeadInfoOnPayBenefits { get; set; }
    }

    public class DeadInfoOnPayBenefit
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Report")]
        public int ReportId { get; set; }
        public virtual LegalReportSection Report { get; set; }


        [Display(Name = "ПИН")]
        [StringLength(maximumLength: 14)]
        public string PIN { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Дата рождения")]
        public DateTime? BirthDate { get; set; }

        [Required]
        [Display(Name = "Пол")]
        [ForeignKey("Gender")]
        public int? GenderId { get; set; }
        public virtual Gender Gender { get; set; }

        [Required]
        [Display(Name = "Справка серия, номер")]
        public string DeadCertificateNo { get; set; }
        
        [Required]
        [Display(Name = "Дата выдачи справки")]
        public DateTime? DeadDateOfCertificate { get; set; }

        [Display(Name = "Гражданство")]
        public string Citizenship { get; set; }

        [Display(Name = "За счет респ. бюджета")]
        public decimal? DeadCadThrowRepublicBudget { get; set; }
    }

    public class BirthInfoOnPayBenefit
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Report")]
        public int ReportId { get; set; }
        public virtual LegalReportSection Report { get; set; }


        [Display(Name = "ПИН")]
        [StringLength(maximumLength: 14)]
        public string PIN { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Дата рождения")]
        public DateTime? BirthDate { get; set; }

        [Required]
        [Display(Name = "Пол")]
        [ForeignKey("Gender")]
        public int? GenderId { get; set; }
        public virtual Gender Gender { get; set; }

        [Display(Name = "Паспорт серия")]
        [StringLength(maximumLength: 3)]
        public string PassportSeries { get; set; }

        [Display(Name = "Паспорт номер")]
        [StringLength(maximumLength: 8)]
        public string PassportNo { get; set; }

        [Display(Name = "Дата выдачи")]
        public DateTime? PassportDate { get; set; }

        [Display(Name = "Выдавший орган")]
        public string PassportOrg { get; set; }

        [Required]
        [Display(Name = "справка серия, номер")]
        public string BirthCertificateNo { get; set; }
        
        [Required]
        [Display(Name = "Период с")]
        public DateTime? DateFrom { get; set; }

        [Required]
        [Display(Name = "Период по")]
        public DateTime? DateTo { get; set; }

        [Display(Name = "Гражданство")]
        public string Citizenship { get; set; }

        
        [Display(Name = "Шестидневка")]
        public bool SixDay { get; set; }
        
        [Display(Name = "Высокогорье")]
        public bool Highlands { get; set; }

        [Required]
        [Display(Name = "Заработная плата за 3 месяца")]
        public decimal? SalaryOf3Months { get; set; }

        [Display(Name = "Количество рабочих дней, на которые начислено пособие (раб. дни)")]
        public int? BirthCountWorkingDays { get; set; }

        [Display(Name = "Размер среднедневного заработка (сом)")]
        public decimal? AmountAverageEarnings { get; set; }

        [Display(Name = "За счет респ. бюджета")]
        public decimal? DeadCadThrowRepublicBudget { get; set; }

        [Display(Name = "За счет собственных средств юридического лица (сом)")]
        public decimal? BirthCashThrowLegalOrg { get; set; }

        [Display(Name = "За счет республиканского бюджета (сом)")]
        public decimal? BirthCadThrowRepublicBudget { get; set; }

        [Display(Name = "Всего (сом)")]
        public virtual decimal? BirthTotalAmount
        {
            get
            {
                return (BirthCashThrowLegalOrg ?? 0) + (BirthCadThrowRepublicBudget ?? 0);
            }
        }

    }


    public class Gender
    {
        public int Id { get; set; }
        [Display(Name = "Код")]
        public int Code { get; set; }
        [Display(Name = "Пол")]
        public string Name { get; set; }
    }

    public class HumDistributionPlan
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [ForeignKey("Company")]
        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [Display(Name = "Дата предоставления плана")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Required]
        public DateTime? Date { get; set; }

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
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? CurrencyRateDate { get; set; }

        [Display(Name = "Дата выдачи заключения")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? CertificateDate { get; set; }

        [Display(Name = "№ заключения")]
        public string CertificateNo { get; set; }

        public virtual ICollection<HumDistributionPlanItem> Items { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Return> Returns { get; set; }
        public virtual ICollection<PlanState> PlanStates { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
    }

    public class HumDistributionPlanItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("HumDistributionPlan")]
        public int? HumDistributionPlanId { get; set; }
        public virtual HumDistributionPlan HumDistributionPlan { get; set; }

        [ForeignKey("Consumer")]
        public int ConsumerId { get; set; }
        public virtual Consumer Consumer { get; set; }

        [ForeignKey("Area")]
        public int AreaId { get; set; }
        public virtual Area Area { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Display(Name = "Полное название")]
        public string ProductFullName { get; set; }

        [Display(Name = "Ед. изм.")]
        [ForeignKey("UnitType")]
        public int UnitTypeId { get; set; }
        public virtual UnitType UnitType { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}"/*"{0:# ### ### ##0,0}"*/)]
        [Display(Name = "Кол-во")]
        [DataType(DataType.Currency)]
        public double? Amount { get; set; }
        
        [Required]
        [Display(Name = "Сумма")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:# ### ### ##0.0}")]
        [DataType(DataType.Currency)]
        public decimal? Sum { get; set; }

        [Display(Name = "Сумма в сомах")]
        //[DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:# ### ### ##0.0}")]
        public decimal? ConvertedSum
        {
            get
            {
                if (HumDistributionPlan != null)
                {
                    if(HumDistributionPlan.CurrencyRate != null)
                    {
                        return Sum * HumDistributionPlan.CurrencyRate;
                    }
                }
                return null;
            }
        }

        public virtual ICollection<ReportItem> ReportItems { get; set; }
        public virtual ICollection<HumDistributionPlanItemChange> HumDistributionPlanItemChanges { get; set; }
    }
    public class HumDistributionPlanItemModel
    {
        public int? HumDistributionPlanId { get; set; }
        public string Consumer { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        
        public int? Amount { get; set; }
        
        public decimal? Sum { get; set; }
    }

    public class Area
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Область")]
        public string Name { get; set; }
    }

    public class District
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Район/Город")]
        public string Name { get; set; }

        [Display(Name = "Область")]
        [ForeignKey("Area")]
        public int AreaId { get; set; }
        public virtual Area Area { get; set; }
    }

    public class Consumer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Потребитель / Организация")]
        public string Name { get; set; }
    }

    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Изделие (категория)")]
        public string Name { get; set; }
    }

    public class Return
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Дата возврата")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ReturnedDate { get; set; }

        [Required]
        [Display(Name = "Комментарий")]
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }

        [ForeignKey("HumDistributionPlan")]
        public int? HumDistributionPlanId { get; set; }
        public virtual HumDistributionPlan HumDistributionPlan { get; set; }
    }

    public class PlanState
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Дата/время установки статуса")]
        public DateTime StateDate { get; set; }

        [Required]
        [ForeignKey("HumDistributionPlan")]
        public int? HumDistributionPlanId { get; set; }
        public virtual HumDistributionPlan HumDistributionPlan { get; set; }

        [Required]
        [ForeignKey("DocumentState")]
        public int? DocumentStateId { get; set; }
        public virtual DocumentState DocumentState { get; set; }
    }

    public class AttachmentType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Документ")]
        public string Name { get; set; }
        public bool IsRequired { get; set; }
    }

    public class Attachment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("AttachmentType")]
        public int? AttachmentTypeId { get; set; }
        public virtual AttachmentType AttachmentType { get; set; }

        [ForeignKey("HumDistributionPlan")]
        public int? HumDistributionPlanId { get; set; }
        public virtual HumDistributionPlan HumDistributionPlan { get; set; }

        [Display(Name = "Размер файла")]
        public string FileSize { get; set; }

        public byte[] Data { get; set; }
    }

    public class HumDistributionPlanItemChange
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("HumDistributionPlanItem")]
        public int? HumDistributionPlanItemId { get; set; }
        public virtual HumDistributionPlanItem HumDistributionPlanItem { get; set; }

        [ForeignKey("Consumer")]
        public int ConsumerId { get; set; }
        public virtual Consumer Consumer { get; set; }

        [ForeignKey("Area")]
        public int AreaId { get; set; }
        public virtual Area Area { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Display(Name = "Ед. изм.")]
        [ForeignKey("UnitType")]
        public int UnitTypeId { get; set; }
        public virtual UnitType UnitType { get; set; }

        [Required]
        //[DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:# ### ### ##0.0}")]
        [Display(Name = "Кол-во")]
        public double? Amount { get; set; }

        [Required]
        [Display(Name = "Сумма")]
        //[DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:# ### ### ##0.0}")]
        public decimal? Sum { get; set; }

        [Display(Name = "Сумма в сомах")]
        //[DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:# ### ### ##0.0}")]
        public decimal? ConvertedSum
        {
            get
            {
                if (HumDistributionPlanItem != null && HumDistributionPlanItem.HumDistributionPlan != null)
                {
                    if (HumDistributionPlanItem.HumDistributionPlan.CurrencyRate != null)
                    {
                        return Sum * HumDistributionPlanItem.HumDistributionPlan.CurrencyRate;
                    }
                }
                return null;
            }
        }
    }


    public class ParentCase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Информация о пациенте")]
        [ForeignKey("Patient")]
        public int? PatientId { get; set; }
        public Patient Patient { get; set; }

        [Display(Name = "Имя, фамилия родителя или законного представителя:")]
        [Required]
        public string FullName { get; set; }

        [Display(Name = "Место заполнения")]
        public string FilledPlaceName { get; set; }

        private DateTime? _filledDate = DateTime.Now;
        [Display(Name = "Дата заполнения")]
        public DateTime? FilledDate { get { return _filledDate; } set { _filledDate = value; } }

        [Display(Name = "Шкала оценки:")]
        [ForeignKey("Reference3")]
        public int? Reference3Id { get; set; }
        public Reference3 Reference3 { get; set; }

        //table
        //Базовые способности ребенка
        [Display(Name = "1.1. Есть ли трудности у ребенка говорить? Почему?")]
        [ForeignKey("Reference4_1_1")]
        public int? Reference4_1_1Id { get; set; }
        public Reference4 Reference4_1_1 { get; set; }

        [Display(Name = "1.2. Как ребенок видит? Есть ли сложности? Почему?")]
        [ForeignKey("Reference4_1_2")]
        public int? Reference4_1_2Id { get; set; }
        public Reference4 Reference4_1_2 { get; set; }

        [Display(Name = "1.3. Как ребенок слышит? Есть ли сложности? Почему?")]
        [ForeignKey("Reference4_1_3")]
        public int? Reference4_1_3Id { get; set; }
        public Reference4 Reference4_1_3 { get; set; }

        [Display(Name = "1.4. Есть ли трудности с рисованием или любой другой интересующей ребенка деятельностью? Мини – тест: поставить точку в центре листа и попросить ребенка нарисовать круги от точки. Нарисуй клубок/колобок/снежок.")]
        [ForeignKey("Reference4_1_4")]
        public int? Reference4_1_4Id { get; set; }
        public Reference4 Reference4_1_4 { get; set; }

        [Display(Name = "1.5. Есть ли сложности у ребенка сделать до конца дело, которое ему поручили? Почему?")]
        [ForeignKey("Reference4_1_5")]
        public int? Reference4_1_5Id { get; set; }
        public Reference4 Reference4_1_5 { get; set; }

        [Display(Name = "1.6. Есть ли трудности с пониманием длины, количества, например: высокий – низкий, толстый – тонкий, длинный – короткий? Мини-тест: предложить ребенку 2 предмета разной высоты.")]
        [ForeignKey("Reference4_1_6")]
        public int? Reference4_1_6Id { get; set; }
        public Reference4 Reference4_1_6 { get; set; }

        [Display(Name = "1.7. Есть ли трудности у ребенка отличать такие понятия как: сладкий – кислый, горячий – холодный, сухой – мокрый, шершавый – гладкий, приятные и неприятные запахи?")]
        [ForeignKey("Reference4_1_7")]
        public int? Reference4_1_7Id { get; set; }
        public Reference4 Reference4_1_7 { get; set; }

        [Display(Name = "1.8. Знает ли цифры? Есть ли трудности в обучении считать?")]
        [ForeignKey("Reference4_1_8")]
        public int? Reference4_1_8Id { get; set; }
        public Reference4 Reference4_1_8 { get; set; }

        [Display(Name = @"1.9. 
0 - 3 года: Знает ли формы: квадрат, круг?
3 - 4 лет: Знает ли буквы?
5 - 6 лет: Есть ли трудности в обучении ребенка читать?
")]
        [ForeignKey("Reference4_1_9")]
        public int? Reference4_1_9Id { get; set; }
        public Reference4 Reference4_1_9 { get; set; }

        //Самостоятельность ребенка
        [Display(Name = "2.1. Есть ли трудности с умыванием? Причина?")]
        [ForeignKey("Reference4_2_1")]
        public int? Reference4_2_1Id { get; set; }
        public Reference4 Reference4_2_1 { get; set; }

        [Display(Name = "2.2. Есть ли трудности почистить зубы? Причина?")]
        [ForeignKey("Reference4_2_2")]
        public int? Reference4_2_2Id { get; set; }
        public Reference4 Reference4_2_2 { get; set; }

        [Display(Name = "2.3. Есть ли трудности самостоятельно одеться? Причина?")]
        [ForeignKey("Reference4_2_3")]
        public int? Reference4_2_3Id { get; set; }
        public Reference4 Reference4_2_3 { get; set; }

        [Display(Name = "2.4. Есть ли трудности самостоятельно пользоваться туалетом? Причина?")]
        [ForeignKey("Reference4_2_4")]
        public int? Reference4_2_4Id { get; set; }
        public Reference4 Reference4_2_4 { get; set; }

        [Display(Name = "2.5. Есть ли трудности самостоятельно есть? Причина? Какую пищу любит ребенок: твердую или жидкую?")]
        [ForeignKey("Reference4_2_5")]
        public int? Reference4_2_5Id { get; set; }
        public Reference4 Reference4_2_5 { get; set; }

        [Display(Name = "2.6. Есть ли трудности при выполнении повседневного распорядка дня?")]
        [ForeignKey("Reference4_2_6")]
        public int? Reference4_2_6Id { get; set; }
        public Reference4 Reference4_2_6 { get; set; }

        [Display(Name = "2.7. Есть ли трудности со сном? Как ребенок спит?")]
        [ForeignKey("Reference4_2_7")]
        public int? Reference4_2_7Id { get; set; }
        public Reference4 Reference4_2_7 { get; set; }

        [Display(Name = "2.8 Внимателен ли ребенок или нет? Есть ли сложности? Почему?")]
        [ForeignKey("Reference4_2_8")]
        public int? Reference4_2_8Id { get; set; }
        public Reference4 Reference4_2_8 { get; set; }

        //Общение
        [Display(Name = "3.1. Есть ли трудности самостоятельно представиться, назвать свое имя?")]
        [ForeignKey("Reference4_3_1")]
        public int? Reference4_3_1Id { get; set; }
        public Reference4 Reference4_3_1 { get; set; }

        [Display(Name = "3.2. Есть ли трудности с пониманием того, что говорят другие?")]
        [ForeignKey("Reference4_3_2")]
        public int? Reference4_3_2Id { get; set; }
        public Reference4 Reference4_3_2 { get; set; }

        [Display(Name = "3.3. Есть ли трудности у ребенка поддержать разговор или же предложить новую тему для общения? – после 5-6 лет")]
        [ForeignKey("Reference4_3_3")]
        public int? Reference4_3_3Id { get; set; }
        public Reference4 Reference4_3_3 { get; set; }

        //Мобильность (движение)
        [Display(Name = "4.1. Есть ли трудности у ребенка самостоятельно встать/сесть?")]
        [ForeignKey("Reference4_4_1")]
        public int? Reference4_4_1Id { get; set; }
        public Reference4 Reference4_4_1 { get; set; }

        [Display(Name = "4.2. Есть ли трудности у ребенка подолгу самостоятельно стоять/сидеть?")]
        [ForeignKey("Reference4_4_2")]
        public int? Reference4_4_2Id { get; set; }
        public Reference4 Reference4_4_2 { get; set; }

        [Display(Name = "4.3. Может ли ребенок взять ручку со стола? Есть ли трудности в управлении кистью руки, пальцев и большого пальца?")]
        [ForeignKey("Reference4_4_3")]
        public int? Reference4_4_3Id { get; set; }
        public Reference4 Reference4_4_3 { get; set; }

        [Display(Name = "4.4. Есть ли трудности при ходьбе? На короткие/длинные расстояния, на ровной/неровной поверхностях, подниматься по ступенькам.")]
        [ForeignKey("Reference4_4_4")]
        public int? Reference4_4_4Id { get; set; }
        public Reference4 Reference4_4_4 { get; set; }

        [Display(Name = "4.5. Есть ли трудности в передвижении из комнаты в комнату, в пределах и вне зданий, вдоль улицы?")]
        [ForeignKey("Reference4_4_5")]
        public int? Reference4_4_5Id { get; set; }
        public Reference4 Reference4_4_5 { get; set; }

        //Межличностные отношения и взаимодействие
        [Display(Name = "5.1. Есть ли трудности в общении с окружающими людьми (например: поздороваться, поблагодарить, извиниться, понять чужие чувства и реагировать на них и т.д.)?")]
        [ForeignKey("Reference4_5_1")]
        public int? Reference4_5_1Id { get; set; }
        public Reference4 Reference4_5_1 { get; set; }

        [Display(Name = "5.2. Есть ли трудности у ребенка проявлять свою привязанность к родным (любит объятия, проявление ласки, жалеет ли маму)?")]
        [ForeignKey("Reference4_5_2")]
        public int? Reference4_5_2Id { get; set; }
        public Reference4 Reference4_5_2 { get; set; }

        //Основные области жизни
        [Display(Name = "6.1. Посещал ли ребенок детский сад? Сформированы ли навыки выполнять домашнее задание?")]
        [ForeignKey("Reference4_6_1")]
        public int? Reference4_6_1Id { get; set; }
        public Reference4 Reference4_6_1 { get; set; }

        [Display(Name = "6.2. С кем ты играешь? С чем? – спросить ребенка. Если не говорит, то родителей.")]
        [ForeignKey("Reference4_6_2")]
        public int? Reference4_6_2Id { get; set; }
        public Reference4 Reference4_6_2 { get; set; }

        [Display(Name = "6.3. Отдых и досуг ребенка: музыка, спорт, концерты  и тп? Какие есть сложности?")]
        [ForeignKey("Reference4_6_3")]
        public int? Reference4_6_3Id { get; set; }
        public Reference4 Reference4_6_3 { get; set; }

        [Display(Name = "6.4. Есть ли у вас трудности с обеспечением средствами ухода за ребенком? В том числе ТСР (технические средства реабилитации)?")]
        [ForeignKey("Reference4_6_4")]
        public int? Reference4_6_4Id { get; set; }
        public Reference4 Reference4_6_4 { get; set; }

        [Display(Name = "6.5. Приспособлен ли Ваш дом под особые потребности ребенка? Почему?")]
        [ForeignKey("Reference4_6_5")]
        public int? Reference4_6_5Id { get; set; }
        public Reference4 Reference4_6_5 { get; set; }

        [Display(Name = "6.6. Приспособлены ли общественные места, где вы часто бываете с ребенком под его особые нужды?")]
        [ForeignKey("Reference4_6_6")]
        public int? Reference4_6_6Id { get; set; }
        public Reference4 Reference4_6_6 { get; set; }

        [Display(Name = "6.7. Приспособлен ли общественный транспорт под особые нужды вашего ребенка?")]
        [ForeignKey("Reference4_6_7")]
        public int? Reference4_6_7Id { get; set; }
        public Reference4 Reference4_6_7 { get; set; }

        [Display(Name = "6.8. Довольны ли Вы качеством медицинских услуг?  Какие у вас есть пожелания?")]
        [ForeignKey("Reference4_6_8")]
        public int? Reference4_6_8Id { get; set; }
        public Reference4 Reference4_6_8 { get; set; }

        [Display(Name = "6.9. Довольны ли Вы качеством услуг работников социальной сферы, образования?  Какие у вас есть пожелания?")]
        [ForeignKey("Reference4_6_9")]
        public int? Reference4_6_9Id { get; set; }
        public Reference4 Reference4_6_9 { get; set; }

    }

    public class Patient
    {
        [Key]
        public int Id { get; set; }
        #region Информация о пациенте
        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }
        [Required]
        [Display(Name = "Тел./Эл.почта")]
        public string Contacts { get; set; }
        [Required]
        [Display(Name = "Адрес")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "ЦСМ")]
        public string CSM { get; set; }
        #endregion
        #region Детали рождения
        [Required]
        [Display(Name = "Дата рождения")]
        public DateTime? BirthDate { get; set; }
        [Required]
        [Display(Name = "Место рождения")]
        public string BirthPlace { get; set; }
        [Required]
        [Display(Name = "Вес при рождении")]
        public double? WeightOnBirth { get; set; }
        [Required]
        [Display(Name = "Возраст, в котором официально поставлен диагноз ДЦП")]
        public int? DiagnosisAge { get; set; }
        #endregion
        #region тип дцп
        [Required]
        [ForeignKey("Spastic")]
        [Display(Name = "Спастика")]
        public int? SpasticId { get; set; }
        public virtual Spastic Spastic { get; set; }
        [Required]
        [ForeignKey("Discinesis")]
        [Display(Name = "Дискинезия")]
        public int? DiscinesisId { get; set; }
        public virtual Discinesis Discinesis { get; set; }
        [Required]
        [ForeignKey("Atacsium")]
        [Display(Name = "Атаксия")]
        public int? AtacsiumId { get; set; }
        public virtual Atacsium Atacsium { get; set; }

        [Display(Name = "Краткий анамнез")]
        [DataType(DataType.MultilineText)]
        public string ShortAnamnez { get; set; }
        #endregion
        #region НАЛИЧИЕ СОПУТСТВУЮЩИХ НАРУШЕНИЙ
        [Required]
        [ForeignKey("Epilepcy")]
        [Display(Name = "Эпилепсия")]
        public int? EpilepcyId { get; set; }
        public virtual Epilepcy Epilepcy { get; set; }

        [Display(Name = "Интеллектуальный")]
        public bool EpilepcyType1 { get; set; }
        [Display(Name = "Визуальный")]
        public bool EpilepcyType2 { get; set; }
        [Display(Name = "Слух")]
        public bool EpilepcyType3 { get; set; }
        [Display(Name = "Речь")]
        public bool EpilepcyType4 { get; set; }
        [Display(Name = "Другое")]
        public string EpilepcyType5 { get; set; }
        #endregion

        //[Required]
        [Display(Name = "СРОКИ ДЦП")]
        [ForeignKey("IcpPeriod")]
        public int? IcpPeriodId { get; set; }
        public virtual IcpPeriod IcpPeriod { get; set; }

        [Required]
        [ForeignKey("IcpReason")]
        [Display(Name = "Была ли подтверждена причина ДЦП?")]
        public int? IcpReasonId { get; set; }
        public virtual IcpReason IcpReason { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "ДИАГНОЗ")]
        public string Diagnosis { get; set; }

        //[Required]
        [Display(Name = "ТЯЖЕСТЬ ДЦП")]
        [ForeignKey("Reference1")]
        public int? Reference1Id { get; set; }
        public virtual Reference1 Reference1 { get; set; }

        //[Required]
        [Display(Name = "Способность пользоваться предметами в быту")]
        [ForeignKey("Reference2")]
        public int? Reference2Id { get; set; }
        public virtual Reference2 Reference2 { get; set; }

    }

    public class Spastic
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Спастика")]
        public string Name { get; set; }
    }

    public class Discinesis
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Дискинезия")]
        public string Name { get; set; }
    }

    public class Atacsium
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Атаксия")]
        public string Name { get; set; }
    }

    public class Epilepcy
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Эпилепсия")]
        public string Name { get; set; }
    }

    public class IcpPeriod
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "СРОКИ ДЦП")]
        public string Name { get; set; }
    }

    public class IcpReason
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Была ли подтверждена причина ДЦП?")]
        public string Name { get; set; }
    }

    public class Reference1
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "ТЯЖЕСТЬ ДЦП")]
        public string Name { get; set; }
    }

    public class Reference2
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Способность пользоваться предметами в быту")]
        public string Name { get; set; }
    }

    public class Reference3
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Шкала оценки:")]
        public string Name { get; set; }
    }

    public class Reference4
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Шкала затруднений:")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Код")]
        public int? Code { get; set; }
    }
    
    public abstract class Code
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Код")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Название")]
        public string FullName { get; set; }
        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }

    [Table("Codes1")]
    public class Code1 : Code//ФУНКЦИИ ОРГАНИЗМА
    {
    }
    
    [Table("Codes2")]
    public class Code2 : Code//СТРУКТУРЫ ОРГАНИЗМА
    {
    }

    [Table("Codes3")]
    public class Code3 : Code//АКТИВНОСТЬ И УЧАСТИЕ
    {
    }

    [Table("Codes4")]
    public class Code4 : Code//ФАКТОРЫ ОКРУЖАЮЩЕЙ СРЕДЫ
    {

    }

    public abstract class Answer
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Question")]
        public int? QuestionId { get; set; }
        public virtual Question Question { get; set; }

        //Источники информации
        [Display(Name = "История болезни")]
        public bool SourceIsCaseHistory { get; set; }
        [Display(Name = "Ответы пациента по вопроснику")]
        public bool SourceIsPatientAnswers { get; set; }
        [Display(Name = "Клинический осмотр")]
        public bool SourceIsClinicalExamination { get; set; }
        [Display(Name = "Техническое обследование")]
        public bool SourceIsTechnicalSurvey { get; set; }

        [Display(Name = "Описание проблемы:")]
        public string ProblemDescription { get; set; }
    }


    public abstract class AnswerValue
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Значение")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Описание")]
        public string Description { get; set; }
    }

    [Table("Answer1Values")]
    public class Answer1Value : AnswerValue
    {
    }

    [Table("Answer2Values1")]
    public class Answer2Value1 : AnswerValue
    {
    }

    [Table("Answer2Values2")]
    public class Answer2Value2 : AnswerValue
    {
    }

    [Table("Answer3Values1")]
    public class Answer3Value1 : AnswerValue
    {
    }

    [Table("Answer3Values2")]
    public class Answer3Value2 : AnswerValue
    {
    }

    [Table("Answer4Values")]
    public class Answer4Value : AnswerValue
    {
    }

    [Table("Answers1")]
    public class Answer1:Answer
    {
        [ForeignKey("Code")]
        public int? CodeId { get; set; }
        public virtual Code1 Code { get; set; }

        [ForeignKey("Value")]
        public int? ValueId { get; set; }
        public virtual Answer1Value Value { get; set; }
    }

    [Table("Answers2")]
    public class Answer2:Answer
    {
        [ForeignKey("Code")]
        public int? CodeId { get; set; }
        public virtual Code2 Code { get; set; }

        [ForeignKey("Value1")]
        public int? ValueId1 { get; set; }
        public virtual Answer2Value1 Value1 { get; set; }
        [ForeignKey("Value2")]
        public int? ValueId2 { get; set; }
        public virtual Answer2Value2 Value2 { get; set; }
    }

    [Table("Answers3")]
    public class Answer3:Answer
    {
        [ForeignKey("Code")]
        public int? CodeId { get; set; }
        public virtual Code3 Code { get; set; }

        [ForeignKey("Value1")]
        public int? ValueId1 { get; set; }
        public virtual Answer3Value1 Value1 { get; set; }
        [ForeignKey("Value2")]
        public int? ValueId2 { get; set; }
        public virtual Answer3Value2 Value2 { get; set; }
    }

    [Table("Answers4")]
    public class Answer4:Answer
    {
        [ForeignKey("Code")]
        public int? CodeId { get; set; }
        public virtual Code4 Code { get; set; }

        [ForeignKey("Value")]
        public int? ValueId { get; set; }
        public virtual Answer4Value Value { get; set; }
    }


    public abstract class AnswerTarget
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("IPR")]
        public int? IPRId { get; set; }
        public virtual IPR IPR { get; set; }
    }

    [Table("AnswerTargets1")]
    public class AnswerTarget1: AnswerTarget
    {
        [ForeignKey("Code")]
        public int? CodeId { get; set; }
        public virtual Code1 Code { get; set; }

        [ForeignKey("Value")]
        public int? ValueId { get; set; }
        public virtual Answer1Value Value { get; set; }
    }

    [Table("AnswerTargets2")]
    public class AnswerTarget2 : AnswerTarget
    {
        [ForeignKey("Code")]
        public int? CodeId { get; set; }
        public virtual Code2 Code { get; set; }

        [ForeignKey("Value1")]
        public int? ValueId1 { get; set; }
        public virtual Answer2Value1 Value1 { get; set; }
        [ForeignKey("Value2")]
        public int? ValueId2 { get; set; }
        public virtual Answer2Value2 Value2 { get; set; }
    }

    [Table("AnswerTargets3")]
    public class AnswerTarget3 : AnswerTarget
    {
        [ForeignKey("Code")]
        public int? CodeId { get; set; }
        public virtual Code3 Code { get; set; }

        [ForeignKey("Value1")]
        public int? ValueId1 { get; set; }
        public virtual Answer3Value1 Value1 { get; set; }
        [ForeignKey("Value2")]
        public int? ValueId2 { get; set; }
        public virtual Answer3Value2 Value2 { get; set; }
    }

    [Table("AnswerTargets4")]
    public class AnswerTarget4 : AnswerTarget
    {
        [ForeignKey("Code")]
        public int? CodeId { get; set; }
        public virtual Code4 Code { get; set; }

        [ForeignKey("Value")]
        public int? ValueId { get; set; }
        public virtual Answer4Value Value { get; set; }
    }

    public abstract class SelectedTarget
    {
        public int Id { get; set; }

        [ForeignKey("IPR")]
        public int? IPRId { get; set; }
        public virtual IPR IPR { get; set; }
    }

    [Table("SelectedTargets1")]
    public class SelectedTarget1 : SelectedTarget
    {
        [ForeignKey("Target")]
        public int? TargetId { get; set; }
        public virtual RehabilitationTarget Target { get; set; }

        [ForeignKey("Code")]
        public int? CodeId { get; set; }
        public virtual Code1 Code { get; set; }
    }

    [Table("SelectedTargets2")]
    public class SelectedTarget2 : SelectedTarget
    {
        [ForeignKey("Target1")]
        public int? TargetId1 { get; set; }
        public virtual RehabilitationTarget Target1 { get; set; }
        [ForeignKey("Target2")]
        public int? TargetId2 { get; set; }
        public virtual RehabilitationTarget Target2 { get; set; }

        [ForeignKey("Code")]
        public int? CodeId { get; set; }
        public virtual Code2 Code { get; set; }
    }

    [Table("SelectedTargets3")]
    public class SelectedTarget3 : SelectedTarget
    {
        [ForeignKey("Target1")]
        public int? TargetId1 { get; set; }
        public virtual RehabilitationTarget Target1 { get; set; }
        [ForeignKey("Target2")]
        public int? TargetId2 { get; set; }
        public virtual RehabilitationTarget Target2 { get; set; }

        [ForeignKey("Code")]
        public int? CodeId { get; set; }
        public virtual Code3 Code { get; set; }
    }

    [Table("SelectedTargets4")]
    public class SelectedTarget4 : SelectedTarget
    {
        [ForeignKey("Target")]
        public int? TargetId { get; set; }
        public virtual RehabilitationTarget Target { get; set; }

        [ForeignKey("Code")]
        public int? CodeId { get; set; }
        public virtual Code4 Code { get; set; }
    }

    public abstract class SelectedService
    {
        public int Id { get; set; }

        [ForeignKey("IPR")]
        public int? IPRId { get; set; }
        public virtual IPR IPR { get; set; }
    }

    [Table("SelectedServices1")]
    public class SelectedService1: SelectedService
    {
        [ForeignKey("Code")]
        public int? CodeId { get; set; }
        public virtual Code1 Code { get; set; }

        [ForeignKey("ServiceType")]
        public int? ServiceTypeId { get; set; }
        public virtual ServiceType ServiceType { get; set; }
    }

    [Table("SelectedServices2")]
    public class SelectedService2 : SelectedService
    {
        [ForeignKey("Code")]
        public int? CodeId { get; set; }
        public virtual Code2 Code { get; set; }

        [ForeignKey("ServiceType1")]
        public int? ServiceTypeId1 { get; set; }
        public virtual ServiceType ServiceType1 { get; set; }

        [ForeignKey("ServiceType2")]
        public int? ServiceTypeId2 { get; set; }
        public virtual ServiceType ServiceType2 { get; set; }
    }

    [Table("SelectedServices3")]
    public class SelectedService3 : SelectedService
    {
        [ForeignKey("Code")]
        public int? CodeId { get; set; }
        public virtual Code3 Code { get; set; }

        [ForeignKey("ServiceType1")]
        public int? ServiceTypeId1 { get; set; }
        public virtual ServiceType ServiceType1 { get; set; }

        [ForeignKey("ServiceType2")]
        public int? ServiceTypeId2 { get; set; }
        public virtual ServiceType ServiceType2 { get; set; }
    }

    [Table("SelectedServices4")]
    public class SelectedService4 : SelectedService
    {
        [ForeignKey("Code")]
        public int? CodeId { get; set; }
        public virtual Code4 Code { get; set; }

        [ForeignKey("ServiceType")]
        public int? ServiceTypeId { get; set; }
        public virtual ServiceType ServiceType { get; set; }
    }

    [Table("Questions")]
    public class Question
    {
        public int Id { get; set; }

        [ForeignKey("Patient")]
        public int? PatientId { get; set; }
        public virtual Patient Patient { get; set; }
    }
    
    [Table("ServiceOrganizations")]
    public class ServiceOrganization
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Наименование и организационно-правовая форма")]
        [DataType(DataType.MultilineText)]
        public string Name { get; set; }
        
        [Required]
        [Display(Name = "Виды инвалидности, с которыми работает организация")]
        [DataType(DataType.MultilineText)]
        public string DisabilityTypes { get; set; }

        [Required]
        [Display(Name = "Возраст ЛОВЗ, с которыми работает организация")]
        public string Ages { get; set; }

        [Required]
        [Display(Name = "Адрес и контактные данные")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Ф.И.О. руководителя")]
        public string ManagerName { get; set; }

        [Display(Name = "Кадровый состав – специалисты")]
        public virtual ICollection<OrgStaff> OrgStaffs { get; set; }

        [Display(Name = "Кол-во специалистов")]
        public int? StaffPositionsCount { get; set; }

        [Display(Name = "Основной источник финансирования")]
        public string FinanceSource { get; set; }

        public virtual ICollection<ServiceType> ServiceTypes { get; set; }
    }

    [Table("ServiceTypes")]
    public class ServiceType
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Вид услуги")]
        [DataType(DataType.MultilineText)]
        public string Name { get; set; }

        [ForeignKey("ServiceOrganization")]
        public int? ServiceOrganizationId { get; set; }
        public ServiceOrganization ServiceOrganization { get; set; }
    }

    [Table("OrgStaffs")]
    public class OrgStaff
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название должности")]
        public string Name { get; set; }

        [ForeignKey("ServiceOrganization")]
        public int? ServiceOrganizationId { get; set; }
        public virtual ServiceOrganization ServiceOrganization { get; set; }
    }

    [Table("Iprs")]
    public class IPR
    {
        public int Id { get; set; }

        [ForeignKey("ParentCase")]
        public int? ParentCaseId { get; set; }
        public virtual ParentCase ParentCase { get; set; }

        [Required]
        [Display(Name = "С:")]
        public DateTime? StartDate { get; set; }
        [Required]
        [Display(Name = "По:")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Акт освидетельствования №:")]
        public string MedicalActNo { get; set; }

        [Display(Name = "Срок очередного освидетельствования:")]
        public DateTime? NextDateExamination { get; set; }


    }

    [Table("RehabilitationTargets")]
    public class RehabilitationTarget
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Цель реабилитации")]
        public string Name { get; set; }
    }

    [Table("RehabilitationTargetToIprs")]
    public class RehabilitationTargetToIPR
    {
        public int Id { get; set; }

        [ForeignKey("RehabilitationTarget")]
        public int? TargetId { get; set; }
        public virtual RehabilitationTarget RehabilitationTarget { get; set; }

        [ForeignKey("IPR")]
        public int? IPRId { get; set; }
        public virtual IPR IPR { get; set; }

        [Display(Name = "Номер цели")]
        public int No { get; set; }
    }

    [Table("CodeServices")]
    public class CodeService
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Код")]
        public string Code { get; set; }
        [ForeignKey("ServiceType")]
        public int? ServiceTypeId { get; set; }
        public virtual ServiceType ServiceType { get; set; }
    }

    [Table("Cases")]
    public class Case
    {
        public int Id { get; set; }

        [Display(Name = "Возраст")]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<CodeCaseBinding> CodeCaseBindings { get; set; }
    }

    [Table("CodeCaseBindings")]
    public class CodeCaseBinding
    {
        public int Id { get; set; }

        [ForeignKey("Case")]
        public int? CaseId { get; set; }
        public virtual Case Case { get; set; }
        public string Code { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Company> Companies { get; set; }
        
        public DbSet<UnitType> UnitTypes { get; set; }

        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportItem> ReportItems { get; set; }
        public DbSet<DocumentState> DocumentStates { get; set; }
        public DbSet<LegalReportSection> LegalReportSections { get; set; }

        public DbSet<DeadInfoOnPayBenefit> DeadInfoOnPayBenefits { get; set; }

        public DbSet<Gender> Genders { get; set; }

        public DbSet<BirthInfoOnPayBenefit> BirthInfoOnPayBenefits { get; set; }

        public DbSet<HumDistributionPlan> HumDistributionPlans { get; set; }

        public DbSet<HumDistributionPlanItem> HumDistributionPlanItems { get; set; }

        public System.Data.Entity.DbSet<IdentitySample.Models.Area> Areas { get; set; }

        public System.Data.Entity.DbSet<IdentitySample.Models.District> Districts { get; set; }

        public System.Data.Entity.DbSet<IdentitySample.Models.Consumer> Consumers { get; set; }

        public System.Data.Entity.DbSet<IdentitySample.Models.Product> Products { get; set; }

        public DbSet<PlanState> PlanStates { get; set; }

        public DbSet<Return> Returns { get; set; }

        public DbSet<AttachmentType> AttachmentTypes { get; set; }

        public DbSet<Attachment> Attachments { get; set; }

        public DbSet<HumDistributionPlanItemChange> HumDistributionPlanItemChanges { get; set; }
        public DbSet<Spastic> Spastics { get; set; }
        public DbSet<Discinesis> Discinesis { get; set; }
        public DbSet<Atacsium> Atacsiums { get; set; }
        public DbSet<Epilepcy> Epilepcies { get; set; }
        public DbSet<IcpPeriod> IcpPeriods { get; set; }
        public DbSet<IcpReason> IcpReasons { get; set; }
        public DbSet<Reference1> Reference1 { get; set; }
        public DbSet<Reference2> Reference2 { get; set; }

        public DbSet<Patient> Patients { get; set; }
        public System.Data.Entity.DbSet<Reference4> Reference4 { get; set; }

        public System.Data.Entity.DbSet<Reference3> Reference3 { get; set; }

        public System.Data.Entity.DbSet<ParentCase> ParentCases { get; set; }
        
        public DbSet<Code1> Codes1 { get; set; }
        public DbSet<Code2> Codes2 { get; set; }
        public DbSet<Code3> Codes3 { get; set; }
        public DbSet<Code4> Codes4 { get; set; }

        public DbSet<Answer1> Answers1 { get; set; }
        public DbSet<Answer2> Answers2 { get; set; }
        public DbSet<Answer3> Answers3 { get; set; }
        public DbSet<Answer4> Answers4 { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Answer1Value> Answer1Values { get; set; }
        public DbSet<Answer2Value1> Answer2Values1 { get; set; }
        public DbSet<Answer2Value2> Answer2Values2 { get; set; }
        public DbSet<Answer3Value1> Answer3Values1 { get; set; }
        public DbSet<Answer3Value2> Answer3Values2 { get; set; }
        public DbSet<Answer4Value> Answer4Values { get; set; }

        public System.Data.Entity.DbSet<IdentitySample.Models.ServiceOrganization> ServiceOrganizations { get; set; }

        public DbSet<IPR> Iprs { get; set; }
        public DbSet<RehabilitationTarget> RehabilitationTargets { get; set; }
        public DbSet<RehabilitationTargetToIPR> RehabilitationTargetToIprs { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }

        public DbSet<AnswerTarget1> AnswerTargets1 { get; set; }
        public DbSet<AnswerTarget2> AnswerTargets2 { get; set; }
        public DbSet<AnswerTarget3> AnswerTargets3 { get; set; }
        public DbSet<AnswerTarget4> AnswerTargets4 { get; set; }

        public DbSet<SelectedTarget1> SelectedTargets1 { get; set; }
        public DbSet<SelectedTarget2> SelectedTargets2 { get; set; }
        public DbSet<SelectedTarget3> SelectedTargets3 { get; set; }
        public DbSet<SelectedTarget4> SelectedTargets4 { get; set; }

        public DbSet<SelectedService1> SelectedServices1 { get; set; }
        public DbSet<SelectedService2> SelectedServices2 { get; set; }
        public DbSet<SelectedService3> SelectedServices3 { get; set; }
        public DbSet<SelectedService4> SelectedServices4 { get; set; }

        public DbSet<OrgStaff> OrgStaffs { get; set; }

        public DbSet<CodeService> CodeServices { get; set; }

        public DbSet<Case> Cases { get; set; }
        public DbSet<CodeCaseBinding> CodeCaseBindings { get; set; }
    }
}