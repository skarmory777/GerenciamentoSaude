using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Origens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Origens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Origens;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class OrigensController : SWMANAGERControllerBase
    {
        private readonly IOrigemAppService _origemAppService;
        private readonly IUnidadeOrganizacionalAppService _unidadeCidadelAppService;

        public OrigensController(
            IOrigemAppService origemAppService,
             IUnidadeOrganizacionalAppService unidadeOrganizacionalAppService
            )
        {
            _origemAppService = origemAppService;
            _unidadeCidadelAppService = unidadeOrganizacionalAppService;
        }

        public async Task<ActionResult> Index()
        {
            var unidadesOrganizacionais = await _unidadeCidadelAppService.Listar(new ListarUnidadesOrganizacionaisInput { MaxResultCount = 50 });

            var model = new OrigensViewModel();
            model.UnidadesOrganizacionais = new SelectList(unidadesOrganizacionais.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0} ({1})", m.Id, m.Descricao) }), "Id", "Descricao");
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Origens/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Origem_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Origem_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            var unidadesOrganizacionais = await _unidadeCidadelAppService.Listar(new ListarUnidadesOrganizacionaisInput { MaxResultCount = 50 });

            CriarOuEditarOrigemModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _origemAppService.Obter((long)id);
                viewModel = new CriarOuEditarOrigemModalViewModel(output);
                viewModel.UnidadesOrganizacionais = new SelectList(unidadesOrganizacionais.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0} ({1})", m.Id, m.Descricao) }), "Id", "Descricao", output.UnidadeOrganizacionalId);
            }
            else
            {
                viewModel = new CriarOuEditarOrigemModalViewModel(new CriarOuEditarOrigem());
                viewModel.UnidadesOrganizacionais = new SelectList(unidadesOrganizacionais.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0} ({1})", m.Id, m.Descricao) }), "Id", "Descricao");
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Origens/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _origemAppService.ListarAutoComplete(term);
            var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}