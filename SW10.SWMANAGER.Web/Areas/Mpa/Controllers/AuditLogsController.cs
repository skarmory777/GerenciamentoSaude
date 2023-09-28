﻿using Abp.Auditing;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.Web.Controllers;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers
{
    [DisableAuditing]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_AuditLogs)]
    public class AuditLogsController : SWMANAGERControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}