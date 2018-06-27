using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;

namespace CISSAPortal.Controllers
{
    public class CodeServicesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CodeServices
        public ActionResult Services(string code)
        {
            var codeServices = db.CodeServices.Where(x => x.Code == code).Include(c => c.ServiceType);
            return PartialView(codeServices.ToList());
        }
        // GET: CodeServices
        public ActionResult Codes(int serviceTypeId)
        {
            var codeServices = db.CodeServices.Where(x => x.ServiceTypeId == serviceTypeId);
            return PartialView(codeServices.ToList());
        }
        
        // GET: CodeServices/Create
        public ActionResult AddService(string code)
        {
            var mySvcs = db.CodeServices.Where(x => x.Code == code).Select(x => x.ServiceTypeId).ToList();
            var svcTypes = db.ServiceTypes.AsQueryable();
            if (mySvcs.Count > 0) svcTypes = svcTypes.Where(x => !mySvcs.Contains(x.Id));

            ViewBag.ServiceTypeId = new SelectList(svcTypes.ToList(), "Id", "Name");

            return View(new CodeService { Code = code });
        }

        // POST: CodeServices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddService([Bind(Include = "Id,Code,ServiceTypeId")] CodeService codeService)
        {
            if (ModelState.IsValid)
            {
                db.CodeServices.Add(codeService);
                db.SaveChanges();
                string codeController = "";
                var codeId = GetCodeId(codeService.Code, out codeController);
                return RedirectToAction("Details", codeController, new { Id = codeId });
            }

            ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes, "Id", "Name", codeService.ServiceTypeId);
            return View(codeService);
        }

        int GetCodeId(string code, out string CodeControllerName)
        {
            var code1 = db.Codes1.FirstOrDefault(x => x.Name == code);
            if (code1 != null)
            {
                CodeControllerName = "Code1";
                return code1.Id;
            }
            var code2 = db.Codes2.FirstOrDefault(x => x.Name == code);
            if (code2 != null)
            {
                CodeControllerName = "Code2";
                return code2.Id;
            }
            var code3 = db.Codes3.FirstOrDefault(x => x.Name == code);
            if (code3 != null)
            {
                CodeControllerName = "Code3";
                return code3.Id;
            }
            var code4 = db.Codes4.FirstOrDefault(x => x.Name == code);
            if (code4 != null)
            {
                CodeControllerName = "Code4";
                return code4.Id;
            }
            throw new ApplicationException("Code not found: " + code);
        }
        
        // GET: CodeServices/Create
        public ActionResult AddCode(int serviceTypeId)
        {
            var myCodes = db.CodeServices.Where(x => x.ServiceTypeId == serviceTypeId).Select(x => x.Code).ToList();
            var codes1 = db.Codes1.AsQueryable();
            if (myCodes.Count > 0) codes1 = codes1.Where(x => !myCodes.Contains(x.Name));

            ViewBag.ServiceTypeId = new SelectList(codes1.ToList(), "Id", "Name");

            return View(new CodeService { ServiceTypeId = serviceTypeId });
        }

        // POST: CodeServices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCode([Bind(Include = "Id,Code,ServiceTypeId")] CodeService codeService)
        {
            if (ModelState.IsValid)
            {
                db.CodeServices.Add(codeService);
                db.SaveChanges();
                string codeController = "";
                var codeId = GetCodeId(codeService.Code, out codeController);
                return RedirectToAction("Details", codeController, new { Id = codeId });
            }

            ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes, "Id", "Name", codeService.ServiceTypeId);
            return View(codeService);
        }

        // GET: CodeServices/Delete/5
        public ActionResult Delete(int id)
        {
            CodeService codeService = db.CodeServices.Find(id);
            if (codeService == null)
            {
                return HttpNotFound();
            }
            db.CodeServices.Remove(codeService);
            db.SaveChanges();
            string codeController = "";
            var codeId = GetCodeId(codeService.Code, out codeController);
            return RedirectToAction("Details", codeController, new { Id = codeId });
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
}
