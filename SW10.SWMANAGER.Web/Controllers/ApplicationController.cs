using Abp.Auditing;
using Abp.Web.Mvc.Authorization;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ApplicationController : SWMANAGERControllerBase
    {
        [DisableAuditing]
        public ActionResult Index()
        {
            /* Enable next line to redirect to Multi-Page Application */
            /* return RedirectToAction("Index", "Home", new {area = "Mpa"}); */

            return RedirectToAction("Index", "Home", new { area = "Mpa" });
        }
    }
}
