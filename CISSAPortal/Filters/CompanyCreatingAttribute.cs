using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CISSAPortal.Filters
{
    public class CompanyCreatingAttribute : ActionFilterAttribute
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated &&
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName != "Account" &&
                filterContext.ActionDescriptor.ActionName != "LogOff" &&
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName != "Companies" &&
                filterContext.ActionDescriptor.ActionName != "Create" &&
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AuthorizeAttribute), false) &&
                (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), false) ||
                filterContext.ActionDescriptor.IsDefined(typeof(AuthorizeAttribute), false)))
            {
                var userId = filterContext.HttpContext.User.Identity.GetUserId();
                var company = db.Companies.FirstOrDefault(x => x.AspNetUserId == userId);
                if(company == null)
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            { "Controller", "Companies" },
                            { "Action", "Create" },
                            { "userId", userId }
                        });
                    base.OnActionExecuting(filterContext);
                }
            }
        }
    }
}