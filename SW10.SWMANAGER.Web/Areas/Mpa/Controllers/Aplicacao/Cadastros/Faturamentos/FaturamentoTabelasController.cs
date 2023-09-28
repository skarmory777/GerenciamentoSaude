using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Tabelas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Tabelas.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.ItensTabela;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.Tabelas;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class FaturamentoTabelasController : SWMANAGERControllerBase
    {
        #region Injecao e Contrutor

        private readonly IFaturamentoTabelaAppService _itemAppService;


        public FaturamentoTabelasController(
            IFaturamentoTabelaAppService itemAppService
            )
        {
            _itemAppService = itemAppService;
        }

        #endregion

        public async Task<ActionResult> Index()
        {
            var model = new FaturamentoTabelasViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Tabelas/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Faturamento_Tabela_Create, AppPermissions.Pages_Tenant_Cadastros_Faturamento_Tabela_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            //var grupos = await _sexoAppService.ListarTodos();
            //var subGrupo = await _sexoAppService.ListarTodos();

            CriarOuEditarFaturamentoTabelaModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _itemAppService.Obter((long)id);
                viewModel = new CriarOuEditarFaturamentoTabelaModalViewModel(output);
                viewModel.ItensModel = new FaturamentoItensTabelaViewModel();
            }
            else
            {
                viewModel = new CriarOuEditarFaturamentoTabelaModalViewModel(new FaturamentoTabelaDto());
                viewModel.ItensModel = new FaturamentoItensTabelaViewModel();
                //viewModel.Grupos = new SelectList(grupos.Select(m => new { Id = grupos.IndexOf(m), Descricao = m }), "Id", "Descricao");

            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Tabelas/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}