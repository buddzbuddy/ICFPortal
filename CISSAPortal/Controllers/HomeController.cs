using IdentitySample.Models;
using Intersoft.CISSA.BizService.Utils;
using Intersoft.CISSA.DataAccessLayer.Core;
using Intersoft.CISSA.DataAccessLayer.Model.Context;
using Intersoft.CISSA.DataAccessLayer.Model.Workflow;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IdentitySample.Controllers
{
    [Authorize]
    public class HomeController : Controller
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

        public async Task<ActionResult> Index()
        {
            //var username = "d";
            //var userId = new Guid("{DCED7BEA-8A93-4BAF-964B-232E75A758C5}");
            var positionId = new Guid("{DF1C36BB-85B0-4C53-8729-F18A5D6615F4}");

            var uManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var userInfo = await uManager.FindByNameAsync(User.Identity.Name);
            /*if(User.Identity.IsAuthenticated && userInfo.Companies != null && userInfo.Companies.Count > 0)
            {
                var company = userInfo.Companies.First();

                //var RGUSOrgId = new Guid("{6853C82D-751E-40DD-AA14-21AF0AB7C64E}");
                var cissameta = new CissaMeta.MetaProxy();
                //var code = cissameta.OrgCode(company.OrgId ?? Guid.Empty);
                var cissa_portal_users = cissameta.GetUsersByPositionId(positionId, company.OrgId ?? Guid.Empty);
                if (cissa_portal_users != null && cissa_portal_users.Count() > 0)
                {
                    var user = cissa_portal_users.First();
                    var context = CreateContext(user.User_Name, user.Id);
                    var ui = context.GetUserInfo();
                    ViewBag.CISSA = ui.Organization.Name + ", username: " + ui.UserName;
                }
            }*/
            
            return View();
        }


        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
