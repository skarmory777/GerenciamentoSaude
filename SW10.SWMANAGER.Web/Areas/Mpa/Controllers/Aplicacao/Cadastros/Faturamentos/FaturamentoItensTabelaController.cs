#region Usings
using Abp.Threading;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItensTabela;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItensTabela.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Tabelas;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.ItensTabela;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
#endregion usings.

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class FaturamentoItensTabelaController : SWMANAGERControllerBase
    {
        #region Cabecalho

        private readonly IFaturamentoItemTabelaAppService _itemTabelaAppService;
        private readonly IFaturamentoTabelaAppService _faturamentoTabelaAppService;
        private readonly IFaturamentoItemAppService _faturamentoItemAppService;
        private readonly ISisMoedaAppService _sisMoedaAppService;


        public FaturamentoItensTabelaController(
            IFaturamentoItemTabelaAppService itemTabelaAppService
            ,
            IFaturamentoTabelaAppService faturamentoTabelaAppService
            ,
            IFaturamentoItemAppService faturamentoItemAppService
            ,
            ISisMoedaAppService sisMoedaAppService
            )
        {
            _itemTabelaAppService = itemTabelaAppService;
            _faturamentoTabelaAppService = faturamentoTabelaAppService;
            _faturamentoItemAppService = faturamentoItemAppService;
            _sisMoedaAppService = sisMoedaAppService;
        }

        #endregion cabecalho.

        public async Task<ActionResult> Index()
        {
            var model = new FaturamentoItensTabelaViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/ItensTabela/Index.cshtml", model);
        }

        //     [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Faturamento_ItemTabela_Create, AppPermissions.Pages_Tenant_Cadastros_Faturamento_ItemTabela_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id, long? tabelaId, long? fatItemId)
        {
            CriarOuEditarFaturamentoItemTabelaModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _itemTabelaAppService.Obter((long)id);
                viewModel = new CriarOuEditarFaturamentoItemTabelaModalViewModel(output);                

            }
            else
            {
                viewModel = new CriarOuEditarFaturamentoItemTabelaModalViewModel(new FaturamentoItemTabelaDto());
                viewModel.IsAtivo = true;
                if (tabelaId != null)
                {
                    viewModel.TabelaId = tabelaId;

                    viewModel.Tabela = AsyncHelper.RunSync(() => _faturamentoTabelaAppService.Obter((long)tabelaId));
                }

                

                if (fatItemId.HasValue)
                {
                    viewModel.ItemId = fatItemId;
                    viewModel.Item = AsyncHelper.RunSync(() => _faturamentoItemAppService.Obter((long)fatItemId));
                }

                viewModel.VigenciaDataInicio = DateTime.Now;
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/ItensTabela/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}