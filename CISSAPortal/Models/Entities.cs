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
        [Display(Name = "Название компании/юр.лица")]
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
        [Required]
        [Display(Name ="Год")]
        public int Year { get; set; }
        [Required]
        [Display(Name = "Месяц")]
        public int Month { get; set; }
        [Required]
        [Display(Name = "Дата")]
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

        public virtual ICollection<ReportItem> ReportItems { get; set; }
    }

    public class ReportItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Report")]
        public int ReportId { get; set; }
        public virtual Report Report { get; set; }

        [Required]
        [Display(Name = "Организация, получившая гум.помощь")]
        public string OrganizationName { get; set; }

        [Required]
        [Display(Name = "Регион")]
        public string Region { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Наименование гум. груза")]
        public string CargoName { get; set; }

        [Required]
        [Display(Name = "Ед. измерения")]
        [ForeignKey("UnitType")]
        public int UnitTypeId { get; set; }
        public virtual UnitType UnitType { get; set; }

        [Required]
        [Display(Name = "Вес (план)")]
        public double PlanedAmount { get; set; }
        [Required]
        [Display(Name = "Сумма (план)")]
        public decimal PlanedSum { get; set; }

        [Display(Name = "Вес (факт)")]
        public double FactAmount { get; set; }
        [Display(Name = "Сумма (факт)")]
        public decimal FactSum { get; set; }

        [Required]
        [Display(Name = "Вес (остаток)")]
        public double BalanceAmount { get; set; }
        [Required]
        [Display(Name = "Сумма (остаток)")]
        public decimal BalanceSum { get; set; }

        [Display(Name = "Вес (резерв для ЧС)")]
        public double ReserveAmount { get; set; }
        [Display(Name = "Сумма (резерв для ЧС)")]
        public decimal ReserveSum { get; set; }
    }

    public class UnitType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ед. измерения")]
        public string Name { get; set; }
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
    }
}