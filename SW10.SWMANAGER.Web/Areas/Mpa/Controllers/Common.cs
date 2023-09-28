using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Common.Modals;
using SW10.SWMANAGER.Web.Controllers;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers
{
    [AbpMvcAuthorize]
    public class CommonController : SWMANAGERControllerBase
    {
        public PartialViewResult LookupModal(LookupModalViewModel model)
        {
            return PartialView("Modals/_LookupModal", model);
        }
    }
}