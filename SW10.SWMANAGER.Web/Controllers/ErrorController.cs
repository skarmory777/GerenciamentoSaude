using Abp.Auditing;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Controllers
{
    public class ErrorController : SWMANAGERControllerBase
    {
        [DisableAuditing]
        public ActionResult E404()
        {
            return View();
        }
    }
}