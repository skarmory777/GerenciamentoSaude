using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Operacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Dashboards;
using SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels;
using SW10.SWMANAGER.Senders.WhatsApp;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Dashboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SW10.SWMANAGER.Web.Controllers;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers
{

    [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class DashboardController : SWMANAGERControllerBase
    {
        private readonly IDashboardAppService _dashboardAppService;

        public DashboardController(IDashboardAppService dashboardAppService)
        {
            _dashboardAppService = dashboardAppService;
        }

        public ActionResult Index()
        {
            return View(new DashboardViewModel());
        }



        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult ListarFaturamentoEntregue()
        {
            try
            {
                var objResult = AsyncHelper.RunSync(() => _dashboardAppService.ListarFaturamentoEntrega());

                return Json(objResult.Items.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ListarFaturamentoAberto()
        {
            try
            {
                var objResult = AsyncHelper.RunSync(() => _dashboardAppService.ListarFaturamentoAberto());

                return Json(objResult.Items.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ListarFaturamentoRecebimento()
        {
            try
            {
                var objResult = AsyncHelper.RunSync(() => _dashboardAppService.ListarFaturamentoRecebimento());

                return Json(objResult.Items.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}