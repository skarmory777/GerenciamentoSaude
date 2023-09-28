using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Web.Controllers;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers
{
    [AbpMvcAuthorize]
    public class WelcomeController : SWMANAGERControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}