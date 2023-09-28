using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposTipoTabelaDominio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposTipoTabelaDominio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTabelaDominio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTabelaDominio.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.GruposTipoTabelaDominio;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class GruposTipoTabelaDominioController : SWMANAGERControllerBase
    {
        private readonly IGrupoTipoTabelaDominioAppService _grupoTipoTabelaDominioAppService;
        private readonly ITipoTabelaDominioAppService _tipoTabelaDominioAppService;

        public GruposTipoTabelaDominioController(
            IGrupoTipoTabelaDominioAppService grupoTipoTabelaDominioAppService,
            ITipoTabelaDominioAppService tipoTabelaDominioAppService
            )
        {
            _grupoTipoTabelaDominioAppService = grupoTipoTabelaDominioAppService;
            _tipoTabelaDominioAppService = tipoTabelaDominioAppService;
        }
        // GET: Mpa/GrupoTipoTabelaDominio
        public async Task<ActionResult> Index()
        {
            var tiposTabelaDominio = await _tipoTabelaDominioAppService.Listar(new ListarTiposTabelaDominioInput { MaxResultCount = 50 });
            var model = new GruposTipoTabelaDominioViewModel();
            model.TiposTabelaDominio = new SelectList(tiposTabelaDominio.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0} ({1})", m.Codigo, m.Descricao) }), "Codigo", "Descricao");
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/GruposTipoTabelaDominio/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_GruposTipoTabelaDominio_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_GruposTipoTabelaDominio_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            var gruposTipoTabelaDominio = await _grupoTipoTabelaDominioAppService.Listar(new ListarGruposTipoTabelaDominioInput { MaxResultCount = 1000 });
            var tiposTabelaDominio = await _tipoTabelaDominioAppService.ListarTodos();
            CriarOuEditarGrupoTipoTabelaDominioModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _grupoTipoTabelaDominioAppService.Obter((long)id);
                viewModel = new CriarOuEditarGrupoTipoTabelaDominioModalViewModel(output);
                viewModel.TiposTabela = new SelectList(tiposTabelaDominio.Items, "Id", "Descricao", output.TipoTabelaDominioId);
            }
            else
            {
                viewModel = new CriarOuEditarGrupoTipoTabelaDominioModalViewModel(new CriarOuEditarGrupoTipoTabelaDominio());
                viewModel.TiposTabela = new SelectList(tiposTabelaDominio.Items, "Id", "Descricao");
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/GruposTipoTabelaDominio/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}