using Abp.Dependency;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItenss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.ContasItens;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Faturamentos
{
    public class FaturamentoContaItensController : SWMANAGERControllerBase
    {
        public ActionResult Index()
        {
            var model = new ContaItensViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/ContaItens/Index.cshtml", model);
        }

        // [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_ContaItem_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_ContaItem_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id, long? contaId = null)
        {
            CriarOuEditarContaItemModalViewModel viewModel;

            // var conta = await _contaAppService.Obter((long)contaId); 

            if (id.HasValue)
            {
                using (var _faturamentoContaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
                {

                    var output = await _faturamentoContaItemAppService.Object.ObterViewModel((long)id);
                    viewModel = new CriarOuEditarContaItemModalViewModel(output);
                    //  viewModel.FaturamentoContaId = contaId;
                    viewModel.FaturamentoItem = output.FatItem;
                    viewModel.CentroCustoDescricao = output.CentroCustoDescricao;
                    viewModel.FaturamentoConfigConvenioId = output.FaturamentoConfigConvenioId;
                    viewModel.ItemCobrado = output.FaturamentoItemCobrado != null ? output.FaturamentoItemCobrado.Descricao : output.FatItem.Descricao;
                }

            }
            else
            {
                viewModel = new CriarOuEditarContaItemModalViewModel(new FaturamentoContaItemDto());
                viewModel.FaturamentoContaId = contaId;
                viewModel.FaturamentoItemId = 0;
                var fatItem = new FaturamentoItemDto();
                fatItem.Grupo = new FaturamentoGrupoDto();
                fatItem.Grupo.TipoGrupoId = 0;
                viewModel.FaturamentoItem = fatItem;
                viewModel.Data = DateTime.Now.Date;//  conta.Atendimento.DataRegistro;
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Faturamentos/ContaItens/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<ActionResult> ContaItem(long? id)
        {
            CriarOuEditarContaItemModalViewModel viewModel;
            if (id.HasValue)
            {
                using (var _faturamentoContaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
                {
                    var output = await _faturamentoContaItemAppService.Object.Obter((long)id);
                    viewModel = new CriarOuEditarContaItemModalViewModel(output);
                }
            }
            else
            {
                viewModel = new CriarOuEditarContaItemModalViewModel(new FaturamentoContaItemDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Faturamentos/ContaItens/ContaItem/Index.cshtml", viewModel);
        }

    }
}