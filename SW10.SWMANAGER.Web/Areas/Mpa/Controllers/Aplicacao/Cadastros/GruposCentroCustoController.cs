using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCentrosCustos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCentrosCustos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.GruposCentroCusto;
using SW10.SWMANAGER.Web.Controllers;

using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class GruposCentroCustoController : SWMANAGERControllerBase
    {
        private readonly IGrupoCentroCustoAppService _grupoCentroCustoAppService;


        public GruposCentroCustoController(
            IGrupoCentroCustoAppService grupoCentroCustoAppService
        )
        {
            _grupoCentroCustoAppService = grupoCentroCustoAppService;
        }

        // GET: Mpa/GruposCentroCusto
        public async Task<ActionResult> Index()
        {
            //  var tiposGrupoCentroCusto = await _grupoCentroCustoAppService.Listar(new ListarGrupoCentroCustosInput{ MaxResultCount = 50 });

            var model = new GruposCentroCustoViewModel();
            //   model.TipoGrupoCentroCustos = new SelectList(tiposGrupoCentroCusto.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1})", m.Id, m.Descricao) }), "Id", "Nome");
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/GruposCentroCusto/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCentroCustos_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCentroCustos_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            //   var tiposGrupoCentroCusto = await _grupoCentroCustoAppService.Listar(new ListarTipoGrupoCentroCustosInput { MaxResultCount = 50 });

            CriarOuEditarGruposCentroCustoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _grupoCentroCustoAppService.Obter((long)id);
                viewModel = new CriarOuEditarGruposCentroCustoModalViewModel(output);
                //   viewModel.TiposGrupoCentroCusto = new SelectList(tiposGrupoCentroCusto.Items.Select(m => new { Id = m.Id, Nome = m.Descricao }), "Id", "Nome", output.TipoGrupoCentroCustosId);
            }
            else
            {
                viewModel = new CriarOuEditarGruposCentroCustoModalViewModel(new CriarOuEditarGrupoCentroCusto());
                //     viewModel.TiposGrupoCentroCusto = new SelectList(tiposGrupoCentroCusto.Items.Select(m => new { Id = m.Id, Nome = m.Descricao }), "Id", "Nome");
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/GruposCentroCusto/_CriarOuEditarModal.cshtml", viewModel);
        }
    }
}