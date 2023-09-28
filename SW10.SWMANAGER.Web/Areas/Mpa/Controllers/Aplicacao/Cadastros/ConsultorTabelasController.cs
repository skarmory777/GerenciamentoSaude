using Abp.Application.Navigation;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ConsultorTabelas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ConsultorTabelas;
using SW10.SWMANAGER.Web.Areas.Mpa.Startup;
using SW10.SWMANAGER.Web.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class ConsultorTabelasController : SWMANAGERControllerBase
    {
        private readonly IConsultorTabelaAppService _consultorTabelaAppService;
        private readonly IConsultorTabelaCampoAppService _consultorTabelaCampoAppService;
        private readonly IConsultorTabelaCampoRelacaoAppService _consultorTabelaCampoRelacaoAppService;
        private readonly IUserNavigationManager _userNavigationManager;

        private readonly IConsultorTipoDadoNFAppService _consultorTipoDadoNFAppService;
        private readonly IConsultorOcorrenciaAppService _consultorOcorrenciaAppService;

        public ConsultorTabelasController(
            IConsultorTabelaAppService consultorTabelaAppService,
            IConsultorTabelaCampoAppService consultorTabelaCampoAppService,
            IConsultorTabelaCampoRelacaoAppService consultorTabelaCampoRelacaoAppService,
            IConsultorTipoDadoNFAppService consultorTipoDadoNFAppService,
            IConsultorOcorrenciaAppService consultorOcorrenciaAppService,
            IUserNavigationManager userNavigationManager
            )
        {
            _consultorTabelaAppService = consultorTabelaAppService;
            _consultorTabelaCampoAppService = consultorTabelaCampoAppService;
            _consultorTabelaCampoRelacaoAppService = consultorTabelaCampoRelacaoAppService;
            _consultorTipoDadoNFAppService = consultorTipoDadoNFAppService;
            _consultorOcorrenciaAppService = consultorOcorrenciaAppService;
            _userNavigationManager = userNavigationManager;
        }

        public async Task<ActionResult> Index()
        {
            var model = new ConsultorTabelasViewModel();
            var campos = await _consultorTabelaCampoAppService.ListarTodos();
            //     var campos = await _consultorTabelaCampoAppService.Listar(new ListarConsultorTabelaCamposInput { MaxResultCount = 100 });
            model.Campos = new SelectList(campos.Items.Select(m => new { Id = m.Id, Codigo = string.Format("{0}", m.Codigo) }), "Id", "Codigo");
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/ConsultorTabelas/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Manutencao_Consultor_Tabela_Create, AppPermissions.Pages_Tenant_Manutencao_Consultor_Tabela_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            //var consultorTabelas = await _consultorTabelaAppService.Listar(new ListarConsultorTabelasInput { MaxResultCount = 1000 });

            var consultorTabelas = await _consultorTabelaAppService.ListarTodos();

            CriarOuEditarConsultorTabelaModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _consultorTabelaAppService.Obter((long)id);
                viewModel = new CriarOuEditarConsultorTabelaModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarConsultorTabelaModalViewModel(new CriarOuEditarConsultorTabela());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/ConsultorTabelas/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<ActionResult> _ConsultorTabelaCampos(long id)
        {
            var viewModel = new ConsultorTabelaCamposViewModel();
            var campos = await _consultorTabelaCampoRelacaoAppService.ListarTabela(id);
            viewModel.ConsultorTabelaCampos = campos.Items.ToList();
            viewModel.TabelaId = id;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/ConsultorTabelas/_ConsultorTabelaCampos.cshtml", viewModel);
        }

        public async Task<ActionResult> _CriarOuEditarConsultorTabelaCamposModal(long consultorTabelaId, long? id)
        {
            CriarOuEditarConsultorTabelaCampoModalViewModel viewModel;

            var consultorTabela = await _consultorTabelaAppService.Obter(consultorTabelaId);
            var campos = await _consultorTabelaCampoRelacaoAppService.ListarCombo(consultorTabelaId);
            var camposDisponiveis = campos.Items.ToList();

            if (id.HasValue)
            {
                var output = await _consultorTabelaCampoAppService.Obter((long)id);
                viewModel = new CriarOuEditarConsultorTabelaCampoModalViewModel(output);
                viewModel.Id = output.Id;
                viewModel.Campos = new SelectList(camposDisponiveis, "Id", "Campo", viewModel.Id);
            }
            else
            {
                viewModel = new CriarOuEditarConsultorTabelaCampoModalViewModel(new ConsultorTabelaCampoDto());
                viewModel.Campos = new SelectList(camposDisponiveis, "Id", "Campo");
            }

            viewModel.ConsultorTabelaId = consultorTabelaId;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/ConsultorTabelas/_CriarOuEditarConsultorTabelaCampoModal.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SalvarConsultorTabelaCampo(CriarOuEditarConsultorTabelaCampo consultorTabelaCampo)
        {
            await _consultorTabelaCampoAppService.CriarOuEditar(consultorTabelaCampo);
            return Content(L("Sucesso"));
        }

        public async Task<ActionResult> ExcluirConsultorTabelaCampo(long id, long tabelaId)
        {
            var consultorTabelaCampo = await _consultorTabelaCampoAppService.Obter(id);
            //   var input = consultorTabelaCampo.MapTo<CriarOuEditarConsultorTabelaCampo>();
            consultorTabelaCampo.ConsultorTabelaId = tabelaId;
            await _consultorTabelaCampoAppService.RemoverRelacaoTabelaCampo(consultorTabelaCampo);
            return Content(L("Sucesso"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SalvarConsultorTabelaCampoRelacao(CriarOuEditarConsultorTabelaCampo consultorTabelaCampo)
        {
            ConsultorTabelaCampoRelacaoDto relacao = new ConsultorTabelaCampoRelacaoDto();
            relacao.ConsultorTabelaId = (long)consultorTabelaCampo.ConsultorTabelaId;
            relacao.ConsultorTabelaCampoId = (long)consultorTabelaCampo.Id;

            // falta Atualizar ConsultorTabelaCampo com novo tabelaId 
            //var campo = _consultorTabelaCampoAppService.Obter(consultorTabelaCampo.Id);
            //_consultorTabelaCampoAppService.CriarOuEditar(campo);

            //////////////

            await _consultorTabelaCampoRelacaoAppService.CriarOuEditar(relacao);
            return Content(L("Sucesso"));
        }

        public async Task<ActionResult> ExcluirConsultorTabelaCampoRelacao(long id)
        {
            var consultorTabelaCampoRelacao = await _consultorTabelaCampoRelacaoAppService.Obter(id);
            // var input = consultorTabelaCampoRelacao.MapTo<ConsultorTabelaCampoRelacaoDto>();
            await _consultorTabelaCampoRelacaoAppService.Excluir(consultorTabelaCampoRelacao);
            return Content(L("Sucesso"));
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Manutencao_Consultor_Tabela_Create, AppPermissions.Pages_Tenant_Manutencao_Consultor_Tabela_Edit)]
        public async Task<PartialViewResult> CriarOuEditarCampo(long? id, long? tabelaId)
        {
            CriarOuEditarConsultorTabelaCampoModalViewModel viewModel;
            var tabelas = await _consultorTabelaAppService.Listar(new ListarConsultorTabelasInput());
            var tiposDadoNF = await _consultorTipoDadoNFAppService.ListarTodos();
            var ocorrencias = await _consultorOcorrenciaAppService.ListarTodos();
            ViewBag.Menu = ObterItensMenu();
            if (id.HasValue)
            {
                var output = await _consultorTabelaCampoAppService.Obter((long)id);
                viewModel = new CriarOuEditarConsultorTabelaCampoModalViewModel(output);
                viewModel.ConsultorTabelas = new SelectList(tabelas.Items, "Id", "Descricao");
                viewModel.ConsultorTiposDadoNF = new SelectList(tiposDadoNF.Items, "Id", "Descricao");
                viewModel.ConsultorOcorrencias = new SelectList(ocorrencias.Items, "Id", "Descricao");
            }
            else
            {
                viewModel = new CriarOuEditarConsultorTabelaCampoModalViewModel(new CriarOuEditarConsultorTabelaCampo());
                viewModel.ConsultorTabelas = new SelectList(tabelas.Items, "Id", "Descricao");
                viewModel.ConsultorTiposDadoNF = new SelectList(tiposDadoNF.Items, "Id", "Descricao");
                viewModel.ConsultorOcorrencias = new SelectList(ocorrencias.Items, "Id", "Descricao");
                viewModel.ConsultorTabelaId = tabelaId;
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/ConsultorTabelas/_EditarCampo.cshtml", viewModel);
        }

        public SelectList ObterItensMenu()
        {
            var fullMenuList = new List<GenericoIdNome>();
            var menu = AsyncHelper.RunSync(() => _userNavigationManager.GetMenuAsync(MpaNavigationProvider.MenuName, AbpSession.ToUserIdentifier()));
            foreach (var menuRaiz in menu.Items)
            {
                var count = -1;
                var fullMenu = new GenericoIdNome();
                string strMenu = menuRaiz.DisplayName;
                if (menuRaiz.Items.Count() > 0)
                {
                    foreach (var MenuNivel1 in menuRaiz.Items)
                    {
                        strMenu += string.Format("/{0}", MenuNivel1.DisplayName);
                        if (MenuNivel1.Items.Count() > 0)
                        {
                            foreach (var MenuNivel2 in menuRaiz.Items)
                            {
                                strMenu += string.Format("/{0}", MenuNivel2.DisplayName);
                                if (MenuNivel2.Items.Count() > 0)
                                {

                                }
                            }
                        }
                    }
                }
                fullMenu.Id = count++;
                fullMenu.Nome = strMenu;
            }
            SelectList cboMenu;
            cboMenu = new SelectList(fullMenuList, "Id", "Nome");
            return cboMenu;
        }

    }
}