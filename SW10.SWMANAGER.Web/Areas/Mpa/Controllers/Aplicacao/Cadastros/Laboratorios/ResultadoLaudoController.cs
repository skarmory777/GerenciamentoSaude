using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosLaudos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosLaudos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.ResultadosLaudos;
using SW10.SWMANAGER.Web.Controllers;

using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Laboratorios
{
    public class ResultadoLaudosController : SWMANAGERControllerBase
    {
        private readonly IResultadoLaudoAppService _ResultadoLaudoAppService;


        public ResultadoLaudosController(IResultadoLaudoAppService ResultadoLaudoAppService)
        {
            _ResultadoLaudoAppService = ResultadoLaudoAppService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new ResultadosLaudosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/ResultadoLaudos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_ResultadoLaudo_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_ResultadoLaudo_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarResultadoLaudoModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _ResultadoLaudoAppService.Obter((long)id);
                viewModel = new CriarOuEditarResultadoLaudoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarResultadoLaudoModalViewModel(new ResultadoLaudoDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/ResultadoLaudos/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _ResultadoLaudoAppService.ListarAutoComplete(term);
            var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            //  return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}