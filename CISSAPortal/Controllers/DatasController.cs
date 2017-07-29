using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CISSAPortal.Controllers
{
    [Authorize]
    public class DatasController : Controller
    {
        // GET: Datas
        public ActionResult Index(bool as_partial = false)
        {
            if (as_partial)
                return PartialView();
            return View();
        }
    }
}