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
        [Display(Name = "Название")]
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
    }

    public class Report
    {
        [Key]
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }
        /// <summary>
        /// Foreign Key
        /// </summary>
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

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
    }
}