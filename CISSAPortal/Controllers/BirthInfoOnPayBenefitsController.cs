using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using Intersoft.CISSA.DataAccessLayer.Model.Workflow;
using Intersoft.CISSA.DataAccessLayer.Model.Query.Builders;
using Intersoft.CISSA.DataAccessLayer.Core;
using Intersoft.CISSA.DataAccessLayer.Model.Context;
using Intersoft.CISSA.BizService.Utils;

namespace CISSAPortal.Controllers
{
    public class BirthInfoOnPayBenefitsController : Controller
    {

        static IAppServiceProvider InitProvider(string username, Guid userId)
        {
            var dataContextFactory = DataContextFactoryProvider.GetFactory();

            var dataContext = dataContextFactory.CreateMultiDc("DataContexts");
            BaseServiceFactory.CreateBaseServiceFactories();
            var providerFactory = AppServiceProviderFactoryProvider.GetFactory();
            var provider = providerFactory.Create(dataContext);
            var serviceRegistrator = provider.Get<IAppServiceProviderRegistrator>();
            serviceRegistrator.AddService(new UserDataProvider(userId, username));
            return provider;
        }

        static WorkflowContext CreateContext(string username, Guid userId)
        {
            return new WorkflowContext(new WorkflowContextData(Guid.Empty, userId), InitProvider(username, userId));
        }
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BirthInfoOnPayBenefits
        public ActionResult Index()
        {
            var birthInfoOnPayBenefits = db.BirthInfoOnPayBenefits.Include(b => b.Gender).Include(b => b.Report);
            return View(birthInfoOnPayBenefits.ToList());
        }

        // GET: BirthInfoOnPayBenefits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BirthInfoOnPayBenefit birthInfoOnPayBenefit = db.BirthInfoOnPayBenefits.Find(id);
            if (birthInfoOnPayBenefit == null)
            {
                return HttpNotFound();
            }
            return View(birthInfoOnPayBenefit);
        }

        // GET: BirthInfoOnPayBenefits/Create
        public ActionResult Create(int reportId)
        {
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name");
            return View(new BirthInfoOnPayBenefit { ReportId = reportId });
        }

        // POST: BirthInfoOnPayBenefits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ReportId,PIN,LastName,FirstName,MiddleName,BirthDate,GenderId,PassportSeries,PassportNo,PassportDate,PassportOrg,BirthCertificateNo,DateFrom,DateTo,Citizenship,DeadCadThrowRepublicBudget,SixDay,Highlands,SalaryOf3Months,BirthCountWorkingDays,AmountAverageEarnings,BirthCashThrowLegalOrg,BirthCadThrowRepublicBudget")] BirthInfoOnPayBenefit birthInfoOnPayBenefit)
        {
            if (ModelState.IsValid)
            {
                var positionId = new Guid("{DF1C36BB-85B0-4C53-8729-F18A5D6615F4}");

                var report = db.LegalReportSections.Find(birthInfoOnPayBenefit.ReportId);
                var company = db.Companies.Find(report.CompanyId);
                if (company != null)
                {
                    //var RGUSOrgId = new Guid("{6853C82D-751E-40DD-AA14-21AF0AB7C64E}");
                    var cissameta = new CissaMeta.MetaProxy();
                    //var code = cissameta.OrgCode(company.OrgId ?? Guid.Empty);
                    var cissa_portal_users = cissameta.GetUsersByPositionId(positionId, company.OrgId ?? Guid.Empty);
                    if (cissa_portal_users != null && cissa_portal_users.Count() > 0)
                    {
                        var user = cissa_portal_users.First();
                        var context = CreateContext(user.User_Name, user.Id);
                        try
                        {
                            CalcBirthInfoOnPayBenefits.Execute(context, birthInfoOnPayBenefit);

                            db.BirthInfoOnPayBenefits.Add(birthInfoOnPayBenefit);
                            db.SaveChanges();
                            return RedirectToAction("Details", "LegalReportSections", new { id = birthInfoOnPayBenefit.ReportId });
                        }
                        catch(Exception e)
                        {
                            ModelState.AddModelError("", e.Message);
                        }
                    }
                }
            }

            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name", birthInfoOnPayBenefit.GenderId);
            return View(birthInfoOnPayBenefit);
        }

        // GET: BirthInfoOnPayBenefits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BirthInfoOnPayBenefit birthInfoOnPayBenefit = db.BirthInfoOnPayBenefits.Find(id);
            if (birthInfoOnPayBenefit == null)
            {
                return HttpNotFound();
            }
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name", birthInfoOnPayBenefit.GenderId);
            ViewBag.ReportId = new SelectList(db.LegalReportSections, "Id", "Id", birthInfoOnPayBenefit.ReportId);
            return View(birthInfoOnPayBenefit);
        }

        // POST: BirthInfoOnPayBenefits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ReportId,PIN,LastName,FirstName,MiddleName,BirthDate,GenderId,PassportSeries,PassportNo,PassportDate,PassportOrg,BirthCertificateNo,DateFrom,DateTo,Citizenship,DeadCadThrowRepublicBudget,SixDay,Highlands,SalaryOf3Months,BirthCountWorkingDays,AmountAverageEarnings,BirthCashThrowLegalOrg,BirthCadThrowRepublicBudget")] BirthInfoOnPayBenefit birthInfoOnPayBenefit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(birthInfoOnPayBenefit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "LegalReportSections", new { id = birthInfoOnPayBenefit.ReportId });
            }
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name", birthInfoOnPayBenefit.GenderId);
            ViewBag.ReportId = new SelectList(db.LegalReportSections, "Id", "Id", birthInfoOnPayBenefit.ReportId);
            return View(birthInfoOnPayBenefit);
        }

        // GET: BirthInfoOnPayBenefits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BirthInfoOnPayBenefit birthInfoOnPayBenefit = db.BirthInfoOnPayBenefits.Find(id);
            if (birthInfoOnPayBenefit == null)
            {
                return HttpNotFound();
            }
            return View(birthInfoOnPayBenefit);
        }

        // POST: BirthInfoOnPayBenefits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BirthInfoOnPayBenefit birthInfoOnPayBenefit = db.BirthInfoOnPayBenefits.Find(id);
            db.BirthInfoOnPayBenefits.Remove(birthInfoOnPayBenefit);
            db.SaveChanges();
            return RedirectToAction("Details", "LegalReportSections", new { id = birthInfoOnPayBenefit.ReportId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
    public static class CalcBirthInfoOnPayBenefits
    {
        public static void Execute(WorkflowContext context, BirthInfoOnPayBenefit birthInfoOnPayBenefit)
        {
            var sixDay = birthInfoOnPayBenefit.SixDay;
            var highlands = birthInfoOnPayBenefit.Highlands;
            
            var dateFrom = birthInfoOnPayBenefit.DateFrom;
            var dateTo = birthInfoOnPayBenefit.DateTo;

            var salary = birthInfoOnPayBenefit.SalaryOf3Months;

            //Расчет по ИП/КФХ, для примера период с 15.09 по 18.01
            var amount = GetAmount(context);
            var fdBeginMonthDay = new DateTime(dateFrom.Value.Year, dateFrom.Value.Month, 1); //01.09
            var fd = fdBeginMonthDay.AddMonths(-3); //01.06
            var holidayDaysPrevM = GetHolidays(context, fd, fdBeginMonthDay); //количество праздничных дней с 01.06 по 31.08
            var workingDaysPrevM = CalcWorkingDays(fd, fdBeginMonthDay); //количество рабочих дней 5 дневка (5д) с 01.06 по 31.08
            var factWorkDaysPrevM = workingDaysPrevM - holidayDaysPrevM - 1; //фактические рабочие дни, за исключением праздников (5д)

            var workingSixDaysPrevM = CalcWorkingSixDays(fd, fdBeginMonthDay); //количество рабочих дней (6д) с 01.06 по 31.08
            var factWorkSixDaysPrevM = workingSixDaysPrevM - holidayDaysPrevM - 1; //фактические рабочие дни, за исключением праздников (6д)

            //расчет количество рабочих дней с 15.09 по 18.01
            var holidayDays = GetHolidays(context, dateFrom.Value, dateTo.Value); //количество праздничных дней с 15.09 по 18.01
            var workingDays = CalcWorkingDays(dateFrom.Value, dateTo.Value); //количество рабочих дней (5д) с 15.09 по 18.01                   
            var factWorkDays = workingDays - holidayDays; //фактические рабочие дни, за исключением праздников (5д)

            var workingSixDays = CalcWorkingSixDays(dateFrom.Value, dateTo.Value); //количество рабочих дней (6д) с 15.09 по 18.01
            var factWorkSixDays = workingSixDays - holidayDays; //фактические рабочие дни, за исключением праздников (6д)

            //расчет количество рабочих дней с 15.09 по 30.09        
            var beginDateFrom = dateFrom; //15.09
            var beginDateTo = new DateTime(beginDateFrom.Value.Year, beginDateFrom.Value.Month, DateTime.DaysInMonth(beginDateFrom.Value.Year, beginDateFrom.Value.Month)); //30.09
            var countBeginDays = CalcWorkingDays(beginDateFrom.Value, beginDateTo); //15.09 по 30.09 (5д)          
            var beginHolidayDays = GetHolidays(context, beginDateFrom.Value, beginDateTo); //кол. праздничных дней с 15.09 по 30.09 
            var beginWorkDays = countBeginDays - beginHolidayDays; //рабочие дни с 15.09 по 30.09 без праздников (5д)

            var countBeginSixDays = CalcWorkingSixDays(beginDateFrom.Value, beginDateTo); //15.09 по 30.09 ( рабочих дней) (6д)
            var beginWorkSixDays = countBeginSixDays - beginHolidayDays; //рабочие дни с 15.09 по 30.09 без праздников (6д)

            //расчет количество рабочих дней с 01.09 по 30.09
            var startMonthDay = new DateTime(beginDateFrom.Value.Year, beginDateFrom.Value.Month, 1); //01.09 
            var countStartDays = CalcWorkingDays(startMonthDay, beginDateTo); //01.09 по 30.09 ( рабочих дней) (5д)            
            var startHolidayDays = GetHolidays(context, startMonthDay, beginDateTo); //кол. праздничных дней с 01.09 по 30.09
            var startWorkDays = countStartDays - startHolidayDays;  //рабочие дни с 01.09 по 30.09 без праздников (5д)

            var countStartSixDays = CalcWorkingSixDays(startMonthDay, beginDateTo); //01.09 по 30.09 ( рабочих дней) (6д)   
            var startWorkSixDays = countStartSixDays - startHolidayDays;  //рабочие дни с 01.09 по 30.09 без праздников (6д)

            //расчет количество рабочих дней с 01.01 по 18.01                                                
            var endDateTo = dateTo; //18.01
            var endDateFrom = new DateTime(endDateTo.Value.Year, endDateTo.Value.Month, 1); //01.01         
            var countEndDays = CalcWorkingDays(endDateFrom, endDateTo.Value); //01.01 по 18.01 (5д)        
            var endHolidayDays = GetHolidays(context, endDateFrom, endDateTo.Value); //кол. праздничных дней с 01.01 по 18.01        
            var endWorkDays = countEndDays - endHolidayDays; //рабочие дни с 01.01 по 18.01 без праздников (5д)       

            var countEndSixDays = CalcWorkingSixDays(endDateFrom, endDateTo.Value); //01.01 по 18.01 (6д)         
            var endWorkSixDays = countEndSixDays - endHolidayDays;  //рабочие дни с 01.01 по 18.01 без праздников (6д)

            //расчет количество рабочих дней с 01.01 по 31.01
            var endMonthFirstDay = new DateTime(endDateFrom.Year, endDateFrom.Month, 1); //01.01 
            var endMonthDateTo = new DateTime(endDateFrom.Year, endDateFrom.Month, DateTime.DaysInMonth(endDateFrom.Year, endDateFrom.Month)); //31.01
            var countEndWorkDays = CalcWorkingDays(endMonthFirstDay, endMonthDateTo); //01.01 по 31.01 ( рабочих дня)(5д)           
            var endFullHolidayDays = GetHolidays(context, endMonthFirstDay, endMonthDateTo); //кол. праздничных дней с 01.01 по 31.01
            var endFullWorkDays = countEndWorkDays - endFullHolidayDays;  //рабочие дни с 01.01 по 31.01 без праздников (5д)        

            var countEndWorkSixDays = CalcWorkingSixDays(endMonthFirstDay, endMonthDateTo); //01.01 по 31.01 ( рабочих дня)(6д)
            var endFullWorkSixDays = countEndWorkSixDays - endFullHolidayDays;  //рабочие дни с 01.01 по 31.01 без праздников (6д)

            //throw new ApplicationException(factWorkSixDaysPrevM.ToString() + " : " + beginWorkSixDays.ToString() + " : " + startWorkSixDays.ToString() + " : " + endWorkSixDays.ToString() + " : " + endFullWorkSixDays.ToString());
            //расчет количество полных месяцев
            var allMonths = (((dateTo.Value.Year - dateFrom.Value.Year) * 12) + (dateTo.Value.Month - dateFrom.Value.Month) + 1);
            var countMonth = allMonths - 2; //2=первый и последний месяц 

            //для высокогорье
            var countMonthSixAmount = factWorkSixDays - beginWorkSixDays;
            var countMonthAmount = factWorkDays - beginWorkDays;


            //calc
            if (sixDay) //6д
            {
                var amountFor3Month = salary / factWorkSixDaysPrevM; //средний зп в день за (июнь, июль, август)
                var tenWorkDayIP = amountFor3Month * 10; //10 раб дней за счет средств работодателя
                var amountDay = (decimal)amount / startWorkSixDays; //заработок в день 1000/21д         
                var firstMonthWD = beginWorkSixDays - 10; //рабочие дни первого месяца c 15.09 по 30.09 - 10 дней        
                var paymentDays = firstMonthWD * amountDay; //оплата за 1 день первого месяца
                var amountMonth = (decimal)amount * countMonth; //каждый месяц по 1000        
                var amountDayLastMonth = (decimal)amount / endFullWorkSixDays; //заработок в день на кпоследний месяц
                var paymentLastDays = amountDayLastMonth * endWorkSixDays;
                var totalAmountRB = paymentDays + amountMonth + paymentLastDays;

                birthInfoOnPayBenefit.BirthCountWorkingDays = factWorkSixDays;
                birthInfoOnPayBenefit.AmountAverageEarnings = Math.Round(amountFor3Month.Value);
                birthInfoOnPayBenefit.BirthCashThrowLegalOrg = Math.Round(tenWorkDayIP.Value); //За счет средств работодателя
                birthInfoOnPayBenefit.BirthCadThrowRepublicBudget = totalAmountRB; //За счет республиканского бюджета
                //obj.BirthTotalAmount = Math.Round((decimal?)appMaternity["BirthCashThrowLegalOrg"] ?? 0) + Math.Round((decimal?)appMaternity["BirthCadThrowRepublicBudget"] ?? 0);
            }
            else //5д
            {
                var amountFor3Month = salary / factWorkDaysPrevM; //средний зп в день за (июнь, июль, август)                      
                var tenWorkDayIP = amountFor3Month * 10; //10 раб дней за счет средств работодателя
                var amountDay = (decimal)amount / startWorkDays; //заработок в день         
                var firstMonthWD = beginWorkDays - 10; //рабочие дни первого месяца c 15.09 по 30.09 
                var paymentDays = firstMonthWD * amountDay; //оплата за 1 день первого месяца                       
                var amountMonth = (decimal)amount * countMonth; //каждый месяц по 1000         
                var amountDayLastMonth = (decimal)amount / endFullWorkDays; //заработок в день на кпоследний месяц 1000/21д = 47,61
                var paymentLastDays = amountDayLastMonth * endWorkDays; //47,61 * 12д = 571,428         
                var totalAmountRB = paymentDays + amountMonth + paymentLastDays; //47,61 + 3000 + 571,428 = 3619,038;

                birthInfoOnPayBenefit.BirthCountWorkingDays = factWorkDays;
                birthInfoOnPayBenefit.AmountAverageEarnings = Math.Round(amountFor3Month.Value);
                birthInfoOnPayBenefit.BirthCashThrowLegalOrg = Math.Round(tenWorkDayIP.Value);
                birthInfoOnPayBenefit.BirthCadThrowRepublicBudget = totalAmountRB;
                //obj.BirthTotalAmount = Math.Round((decimal?)appMaternity["BirthCashThrowLegalOrg"] ?? 0) + Math.Round((decimal?)appMaternity["BirthCadThrowRepublicBudget"] ?? 0);
            }
            //высокогорье
            if (highlands)
            {
                if (sixDay) //6д
                {
                    var amountFor3Month = salary / factWorkSixDaysPrevM;
                    var tenWorkDayIP = amountFor3Month * 10; //10 раб дней за счет средств работодателя
                    var firstMonthWD = beginWorkSixDays - 10;
                    var amountDay = amountFor3Month * firstMonthWD;
                    var allMonthAmount = countMonthSixAmount * amountFor3Month;
                    var totalAmountRB = amountDay + allMonthAmount;

                    birthInfoOnPayBenefit.BirthCountWorkingDays = factWorkSixDays;
                    birthInfoOnPayBenefit.AmountAverageEarnings = Math.Round(amountFor3Month.Value);
                    birthInfoOnPayBenefit.BirthCashThrowLegalOrg = Math.Round(tenWorkDayIP.Value);
                    birthInfoOnPayBenefit.BirthCadThrowRepublicBudget = totalAmountRB;
                    //obj.BirthTotalAmount = Math.Round((decimal?)appMaternity["BirthCashThrowLegalOrg"] ?? 0) + Math.Round((decimal?)appMaternity["BirthCadThrowRepublicBudget"] ?? 0);
                }
                else //5д                                         
                {
                    var amountFor3Month = salary / factWorkDaysPrevM;
                    var tenWorkDayIP = amountFor3Month * 10; //10 раб дней за счет средств работодателя
                    var firstMonthWD = beginWorkDays - 10;
                    var amountDay = amountFor3Month * firstMonthWD;
                    var allMonthAmount = countMonthAmount * amountFor3Month;
                    var totalAmountRB = amountDay + allMonthAmount;

                    birthInfoOnPayBenefit.BirthCountWorkingDays = factWorkDays;
                    birthInfoOnPayBenefit.AmountAverageEarnings = Math.Round(amountFor3Month.Value);
                    birthInfoOnPayBenefit.BirthCashThrowLegalOrg = Math.Round(tenWorkDayIP.Value);
                    birthInfoOnPayBenefit.BirthCadThrowRepublicBudget = totalAmountRB;
                    //obj.BirthTotalAmount = Math.Round((decimal?)appMaternity["BirthCashThrowLegalOrg"] ?? 0) + Math.Round((decimal?)appMaternity["BirthCadThrowRepublicBudget"] ?? 0);
                }
            }
        }
        /***********************************************************************************************************************/
        //Размер пособия по беременности/родом и для ритуального погребения
        private static readonly Guid AmountRitualBenefitDefId = new Guid("{07031123-E5DA-4D8B-AE85-446F9981E5A5}");
        private static readonly Guid TypePaysEnumId = new Guid("{40F1C4F4-3BAA-4348-B881-AFD5F69B676F}"); //беременность/роды спр.
        private static decimal GetAmount(WorkflowContext context)
        {
            var today = DateTime.Today;
            var aqb = new QueryBuilder(AmountRitualBenefitDefId);
            aqb.Where("From").Le(today).And("To").Gt(today).And("TypePays").Eq(TypePaysEnumId);
            using (var query = context.CreateSqlQuery(aqb.Def))
            {
                query.AddAttribute("Amount");
                query.AddOrderAttribute("From", false);
                using (var reader = context.CreateSqlReader(query))
                {
                    if (!reader.Read())
                        throw new ApplicationException("Размер пособия в справочнике не указан!");
                    return reader.Reader.GetDecimal(0);
                }
            }
        }
        /***********************************************************************************************************************/
        //Праздничные дни спр.
        static string holidayDates = "";
        private static int GetHolidays(WorkflowContext context, DateTime dateFrom, DateTime dateTo, bool withLog = false)
        {
            holidayDates = "";
            var holidayDefId = new Guid("{22ECC24B-1770-467C-8F75-C79D69A2CF95}");
            var qb = new QueryBuilder(holidayDefId, context.UserId);
            qb.Where("AnnualHolidays").IsNotNull().And("DateOfHolidays").IsNotNull();
            var query = context.CreateSqlQuery(qb.Def);
            query.AddAttribute("DateOfHolidays");
            int count = 0;
            using (var reader = context.CreateSqlReader(query))
            {
                while (reader.Read())
                {
                    var date = reader.GetDateTime(0);
                    int defYear = 0001;
                    var dateDef = new DateTime(defYear, date.Month, date.Day);
                    var dateFromDef = new DateTime(defYear, dateFrom.Month, dateFrom.Day);
                    var dateToDef = new DateTime(defYear, dateTo.Month, dateTo.Day);
                    if (dateFromDef > dateDef) dateDef = dateDef.AddYears(1);
                    dateToDef = new DateTime(dateFrom.Year < dateTo.Year ? defYear + 1 : defYear, dateTo.Month, dateTo.Day);

                    if (dateDef >= dateFromDef && dateDef <= dateToDef)
                    {
                        count++;
                        if (withLog)
                            holidayDates += " [" + dateDef.ToShortDateString() + "] ";
                    }
                    else
                    {
                        if (withLog)
                            holidayDates += " !(" + dateDef.ToShortDateString() + " >= " + dateFromDef.ToShortDateString() + " && " + dateDef.ToShortDateString() + " <= " + dateToDef.ToShortDateString() + ") ";
                    }
                }
            }
            qb = new QueryBuilder(holidayDefId, context.UserId);
            qb.Where("AnnualHolidays").IsNull().And("DateOfHolidays").Ge(dateFrom).And("DateOfHolidays").Le(dateTo);
            query = context.CreateSqlQuery(qb.Def);
            query.AddAttribute("&Id", Intersoft.CISSA.DataAccessLayer.Model.Query.Sql.SqlQuerySummaryFunction.Count);
            using (var reader = context.CreateSqlReader(query))
            {
                if (reader.Read())
                {
                    count += reader.GetInt32(0);
                }
            }
            return count;
        }
        /***********************************************************************************************************************/
        //метод вывода количество рабочих дней за исключением субботу и воскресенье
        private static int CalcWorkingDays(DateTime dateFrom, DateTime dateTo)
        {
            if (dateFrom >= dateTo)
                throw new ArgumentException("Срок является недопустимым для расчета рабочих дней");
            var day = dateFrom;
            var days = 0;
            while (day <= dateTo)
            {
                if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
                {
                    days++;
                }
                day = day.AddDays(1);
            }
            return days;
        }
        /***********************************************************************************************************************/
        //метод вывода количество рабочих дней за исключением воскресенье
        private static int CalcWorkingSixDays(DateTime dateFrom, DateTime dateTo)
        {
            if (dateFrom >= dateTo)
                throw new ArgumentException("Срок является недопустимым для расчета рабочих дней");
            var daySix = dateFrom;
            var sixDays = 0;
            while (daySix <= dateTo)
            {
                if (daySix.DayOfWeek != DayOfWeek.Sunday)
                {
                    sixDays++;
                }
                daySix = daySix.AddDays(1);
            }
            return sixDays;
        }
    }
}
