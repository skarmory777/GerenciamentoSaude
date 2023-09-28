using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TabelasResultados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TabelasResultados.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.TabelasResultados;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Laboratorios
{
    public class TabelasResultadosController : SWMANAGERControllerBase
    {
        private readonly ITabelaResultadoAppService _TabelaResultadoAppService;

        public TabelasResultadosController(
            ITabelaResultadoAppService TabelaResultadoAppService
            )
        {
            _TabelaResultadoAppService = TabelaResultadoAppService;
        }

        public PartialViewResult Index(int? tabelaId)
        {
            var model = new TabelasResultadosViewModel();
            model.TabelaId = tabelaId;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/TabelaResultados/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_TabelaResultado_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_TabelaResultado_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id, long? tabelaId)
        {
            CriarOuEditarTabelaResultadoModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _TabelaResultadoAppService.Obter((long)id);
                viewModel = new CriarOuEditarTabelaResultadoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarTabelaResultadoModalViewModel(new TabelaResultadoDto());
                viewModel.TabelaId = tabelaId;
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/TabelaResultados/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _TabelaResultadoAppService.ListarAutoComplete(term);
            var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            //  return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}