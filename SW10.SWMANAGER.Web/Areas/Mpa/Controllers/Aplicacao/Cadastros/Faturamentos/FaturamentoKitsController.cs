#region Usings
using Abp.Threading;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Kits;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Kits.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.Kits;
using SW10.SWMANAGER.Web.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
#endregion usings.

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class FaturamentoKitsController : SWMANAGERControllerBase
    {
        #region Injecao e Contrutor

        private readonly IFaturamentoKitAppService _faturamentoKitAppService;



        public FaturamentoKitsController(
            IFaturamentoKitAppService faturamentoKitAppService
            )
        {
            _faturamentoKitAppService = faturamentoKitAppService;
        }

        #endregion

        public async Task<ActionResult> Index()
        {
            var model = new FaturamentoKitsViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Kits/Index.cshtml", model);
        }

        //[AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Faturamento_Kit_Create, AppPermissions.Pages_Tenant_Cadastros_Faturamento_Kit_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarFaturamentoKitModalViewModel viewModel;
            List<FaturamentoItemQuantidade> itensQuantidade = new List<FaturamentoItemQuantidade>();

            if (id.HasValue)
            {
                var output = await _faturamentoKitAppService.Obter((long)id).ConfigureAwait(false);

                foreach (var item in output.Itens)
                {
                    itensQuantidade.Add(new FaturamentoItemQuantidade { 
                        ItemId = item.FatItemId ?? 0, 
                        Quantidade = item.Quantidade,
                        Codigo = item.FatItem?.Codigo,
                        Descricao = item.FatItem?.Descricao
                    });
                }

                output.StrItensQtds = JsonConvert.SerializeObject(itensQuantidade);

                viewModel = new CriarOuEditarFaturamentoKitModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarFaturamentoKitModalViewModel(new FaturamentoKitDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Kits/_CriarOuEditarModal.cshtml", viewModel);
        }


        // PARCIAIS
        // Honorarios
        public PartialViewResult ConfigHonorarios(long? itemId)
        {
            CriarOuEditarFaturamentoKitModalViewModel viewModel;
            if (itemId.HasValue)
            {
                var output = AsyncHelper.RunSync(() => _faturamentoKitAppService.Obter((long)itemId));
                //   model = AsyncHelper.RunSync(() => _faturamentoKitAppService.Obter((long)itemId)).MapTo<CriarOuEditarPaciente>();
                //  var output = _faturamentoKitAppService.Obter((long)itemId);
                viewModel = new CriarOuEditarFaturamentoKitModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarFaturamentoKitModalViewModel(new FaturamentoKitDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Kits/Configuracoes/_Honorarios.cshtml", viewModel);
        }

        // Servicos
        public PartialViewResult ConfigServicos(long? itemId)
        {
            CriarOuEditarFaturamentoKitModalViewModel viewModel;
            if (itemId.HasValue)
            {
                var output = _faturamentoKitAppService.Obter((long)itemId);
                viewModel = new CriarOuEditarFaturamentoKitModalViewModel(output.Result);
            }
            else
            {
                viewModel = new CriarOuEditarFaturamentoKitModalViewModel(new FaturamentoKitDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Kits/Configuracoes/_Servicos.cshtml", viewModel);
        }

        // Servicos
        public PartialViewResult ConfigProdutos(long? itemId)
        {
            CriarOuEditarFaturamentoKitModalViewModel viewModel;
            if (itemId.HasValue)
            {
                var output = _faturamentoKitAppService.Obter((long)itemId);
                viewModel = new CriarOuEditarFaturamentoKitModalViewModel(output.Result);
            }
            else
            {
                viewModel = new CriarOuEditarFaturamentoKitModalViewModel(new FaturamentoKitDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Kits/Configuracoes/_Produtos.cshtml", viewModel);
        }

        // Pacotes
        public PartialViewResult ConfigPacotes(long? itemId)
        {
            CriarOuEditarFaturamentoKitModalViewModel viewModel;
            if (itemId.HasValue)
            {
                var output = _faturamentoKitAppService.Obter((long)itemId);
                viewModel = new CriarOuEditarFaturamentoKitModalViewModel(output.Result);
            }
            else
            {
                viewModel = new CriarOuEditarFaturamentoKitModalViewModel(new FaturamentoKitDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Kits/Configuracoes/_Pacotes.cshtml", viewModel);
        }

    }
}