using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposTipoTabelaDominio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTabelaDominio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTabelaDominio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VersoesTiss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VersoesTiss.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TabelasDominio;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class TabelasDominioController : SWMANAGERControllerBase
    {
        private readonly ITabelaDominioAppService _tabelaDominioAppService;
        private readonly ITipoTabelaDominioAppService _tipoTabelaDominioAppService;
        private readonly IGrupoTipoTabelaDominioAppService _grupoTipoTabelaDominioAppService;
        private readonly IVersaoTissAppService _versaoTissAppService;
        private readonly ITabelaDominioVersaoTissAppService _tabelaDominioVersaoTissAppService;

        public TabelasDominioController(
            ITabelaDominioAppService tabelaDominioAppService,
            ITipoTabelaDominioAppService tipoTabelaDominioAppService,
            IGrupoTipoTabelaDominioAppService grupoTipoTabelaDominioAppService,
            IVersaoTissAppService versaoTissAppService,
            ITabelaDominioVersaoTissAppService tabelaDominioVersaoTissAppService
            )
        {
            _tabelaDominioAppService = tabelaDominioAppService;
            _tipoTabelaDominioAppService = tipoTabelaDominioAppService;
            _grupoTipoTabelaDominioAppService = grupoTipoTabelaDominioAppService;
            _versaoTissAppService = versaoTissAppService;
            _tabelaDominioVersaoTissAppService = tabelaDominioVersaoTissAppService;
        }
        // GET: Mpa/TabelaDominio
        public async Task<ActionResult> Index()
        {
            var tiposTabela = await _tipoTabelaDominioAppService.Listar(new ListarTiposTabelaDominioInput { MaxResultCount = 50 });
            var versoesTiss = await _versaoTissAppService.Listar(new ListarVersoesTissInput { MaxResultCount = 100 });
            var model = new TabelasDominioViewModel();
            model.TiposTabela = new SelectList(tiposTabela.Items.Select(m => new { Id = m.Id, Codigo = string.Format("{0} - {1}", m.Codigo, m.Descricao) }), "Id", "Codigo");
            model.VersoesTiss = new SelectList(versoesTiss.Items.Select(m => new { Id = m.Id, Codigo = string.Format("{0}", m.Codigo) }), "Id", "Codigo");
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/TabelasDominio/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TabelasDominio_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TabelasDominio_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            var tabelasDominio = await _tabelaDominioAppService.Listar(new ListarTabelasDominioInput { MaxResultCount = 1000 });
            var tiposTabelaDominio = await _tipoTabelaDominioAppService.ListarTodos();

            CriarOuEditarTabelaDominioModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _tabelaDominioAppService.Obter((long)id);
                viewModel = new CriarOuEditarTabelaDominioModalViewModel(output);

                var gruposTipoTabelaDominio = await _grupoTipoTabelaDominioAppService.ListarPorTipo((long)output.TipoTabelaDominioId);
                var versoesTiss = await _versaoTissAppService.ListarPorTabelaDominio(output.Id);

                viewModel.VersoesTiss = versoesTiss;
                viewModel.TiposTabela = new SelectList(tiposTabelaDominio.Items, "Id", "Descricao", output.TipoTabelaDominioId);

                if (gruposTipoTabelaDominio.Items.Count() > 0)
                {
                    viewModel.GruposTipoTabelaDominio = new SelectList(gruposTipoTabelaDominio.Items, "Id", "Descricao", output.GrupoTipoTabelaDominioId);
                }
            }
            else
            {
                viewModel = new CriarOuEditarTabelaDominioModalViewModel(new TabelaDominioDto());
                viewModel.TiposTabela = new SelectList(tiposTabelaDominio.Items, "Id", "Descricao");
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/TabelasDominio/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<PartialViewResult> _MontarComboTabelaDominioGrupos(long tabelaDominioId)
        {
            var grupos = await _grupoTipoTabelaDominioAppService.ListarPorTipo(tabelaDominioId);
            var grupoId = string.Empty;
            if (grupos.Items.Count() == 1)
            {
                grupoId = grupos.Items.FirstOrDefault().Id.ToString();
            }
            MontarComboTabelaDominioGruposViewModel viewModel = new MontarComboTabelaDominioGruposViewModel();
            viewModel.GruposTipoTabelaDominio = new SelectList(grupos.Items, "Id", "Descricao", grupoId);
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/TabelasDominio/_MontarComboTabelaDominioGrupos.cshtml", viewModel);
        }

        public async Task<ActionResult> _TabelaDominioVersoesTiss(long id)
        {
            var versoesTiss = await _versaoTissAppService.ListarTodos();
            var result = await _tabelaDominioAppService.Obter(id);

            //var tabelaDominioVersoesTiss = result.TabelaDominioVersoesTiss != null ? result.TabelaDominioVersoesTiss.ToList() : new List<TabelaDominioVersaoTissDto>();

            var viewModel = new TabelaDominioVersoesTissViewModel();
            viewModel.VersoesTiss = versoesTiss.Items.ToList();
            //viewModel.TabelaDominioVersoesTiss = tabelaDominioVersoesTiss;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/TabelasDominio/_TabelaDominioVersoesTiss.cshtml", viewModel);
        }

        public async Task<ActionResult> _CriarOuEditarTabelaDominioVersaoTissModal(long tabelaDominioId, long? id)
        {
            CriarOuEditarTabelaDominioVersaoTissModalViewModel viewModel;
            var tabelaDominio = await _tabelaDominioAppService.Obter(tabelaDominioId);
            var versoesTiss = await _versaoTissAppService.ListarTodos();
            //var tabelaDominioVersoesTiss = tabelaDominio.TabelaDominioVersoesTiss;
            //var versoesTissCadastradas = tabelaDominioVersoesTiss.Select(m => m.VersaoTissId);

            //var versoesTissTabelaDominio = versoesTiss.Items.Where(m => m.Id.IsIn(versoesTissCadastradas.ToArray()));
            //var versoesTissDisponiveis = versoesTiss.Items.Except(versoesTissTabelaDominio).ToList();
            if (id.HasValue)
            {
                var output = await _tabelaDominioVersaoTissAppService.Obter((long)id);
                viewModel = new CriarOuEditarTabelaDominioVersaoTissModalViewModel(output);
                var versaoTiss = await _versaoTissAppService.Obter(output.VersaoTissId);
                //viewModel.VersaoTiss = versaoTiss;
                viewModel.VersaoTissId = output.VersaoTissId;
                //viewModel.VersoesTiss = new SelectList(versoesTissDisponiveis, "Id", "Codigo", viewModel.VersaoTissId);
            }
            else
            {
                viewModel = new CriarOuEditarTabelaDominioVersaoTissModalViewModel(new TabelaDominioVersaoTissDto());
                //viewModel.VersoesTiss = new SelectList(versoesTissDisponiveis, "Id", "Codigo");
            }
            //viewModel.TabelaDominio = tabelaDominio;
            viewModel.VersaoTissId = tabelaDominioId;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/TabelasDominio/_CriarOuEditarTabelaDominioVersaoTissModal.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SalvarTabelaDominioVersaoTiss(TabelaDominioVersaoTissDto tabelaDominioVersaoTiss)
        {
            await _tabelaDominioVersaoTissAppService.CriarOuEditar(tabelaDominioVersaoTiss);
            return Content(L("Sucesso"));
        }

        public async Task<ActionResult> ExcluirTabelaDominioVersaoTiss(long id)
        {
            var tabelaDominioVersaoTiss = await _tabelaDominioVersaoTissAppService.Obter(id);
            await _tabelaDominioVersaoTissAppService.Excluir(tabelaDominioVersaoTiss);
            return Content(L("Sucesso"));
        }


    }
}