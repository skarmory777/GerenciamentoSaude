using Abp.Web.Mvc.Authorization;
using Newtonsoft.Json;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tabelas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tabelas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TabelasResultados.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.Tabelas;
using SW10.SWMANAGER.Web.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Laboratorios
{
    public class TabelasController : SWMANAGERControllerBase
    {
        private readonly ITabelaAppService _TabelaAppService;

        public TabelasController(
            ITabelaAppService TabelaAppService
            )
        {
            _TabelaAppService = TabelaAppService;
        }

        public ActionResult Index()
        {
            var model = new TabelasViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Tabelas/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Tabela_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Tabela_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarTabelaModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _TabelaAppService.Obter((long)id);
                viewModel = new CriarOuEditarTabelaModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarTabelaModalViewModel(new TabelaDto());
                viewModel.Resultados = JsonConvert.SerializeObject(new List<TabelaResultadoDto>());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Tabelas/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _TabelaAppService.ListarAutoComplete(term);
            var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            //  return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}