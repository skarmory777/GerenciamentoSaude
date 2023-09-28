using Abp.Dependency;
using Abp.Runtime.Session;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Suprimentos.Estoques
{
    public class ConfirmacaoMovimentosController : SWMANAGERControllerBase
    {
        // GET: Mpa/ConfirmacaoMovimentos
        public async Task<ActionResult> Index()
        {
            var model = new PreMovimentoViewModel();

            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/ConfirmacaoMovimentos/Index.cshtml", model);
        }

        public async Task<ActionResult> ConfirmarEntradaModal(long? id)
        {
            CriarOuEditarPreMovimentoModalViewModel viewModel = null;

            if (id.HasValue) //edição
            {
                using (var _preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
                {
                    EstoquePreMovimentoDto output = await _preMovimentoAppService.Object.Obter((long)id);
                    viewModel = new CriarOuEditarPreMovimentoModalViewModel(output);
                }
            }

            viewModel.Movimento = DateTime.Now;
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/ConfirmacaoMovimentos/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<ActionResult> InformarLoteValidadeProdutoModal(long preMovimentoItemId, long produtoId)
        {
            var viewModel = new CriarOuEditarPreMovimentoItemModalViewModel(new EstoquePreMovimentoItemDto { PreMovimentoId = preMovimentoItemId, ProdutoId = produtoId });
            using (var _estoquePreMovimentoItemAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoItemAppService>())
            using (var _preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            {
                var preMovimentoItem = _estoquePreMovimentoItemAppService.Object.Obter(preMovimentoItemId).Result;
                if (preMovimentoItem != null)
                {
                    preMovimentoItem.EstoquePreMovimento = await _preMovimentoAppService.Object.Obter(preMovimentoItem.PreMovimentoId);
                    if (preMovimentoItem.EstoquePreMovimento != null)
                    {
                        viewModel.PreMovimentoEstadoId = preMovimentoItem.EstoquePreMovimento.PreMovimentoEstadoId;
                    }

                    viewModel.Quantidade = preMovimentoItem.Quantidade;

                    viewModel.Produto = preMovimentoItem.Produto;
                }
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/ConfirmacaoMovimentos/_InformarLoteValidadeProduto.cshtml", viewModel);
            }
        }

        public async Task<ActionResult> SaidaModal(long? id)
        {
            using (var _preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            using (var abpSession = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
            {
                var userId = abpSession.Object.UserId.Value;
                CriarOuEditarPreMovimentoModalViewModel viewModel;

                if (id.HasValue) //edição
                {
                    EstoquePreMovimentoDto output = await _preMovimentoAppService.Object.Obter((long)id);
                    viewModel = new CriarOuEditarPreMovimentoModalViewModel(output);
                    viewModel.PermiteConfirmacaoEntrada = await _preMovimentoAppService.Object.PermiteConfirmarEntrada(output);
                }
                else //Novo
                {
                    viewModel = new CriarOuEditarPreMovimentoModalViewModel(new PreMovimentoViewModel());
                }

                viewModel.Movimento = DateTime.Now;
                return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/ConfirmacaoMovimentos/_SaidaModal.cshtml", viewModel);
            }
        }

        public async Task<ActionResult> TransferenciaModal(long? id)
        {
            EstoqueTransferenciaProdutoViewModel viewModel;

            if (id.HasValue) //edição
            {
                using (var _preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
                {
                    EstoqueTransferenciaProdutoDto output = await _preMovimentoAppService.Object.ObterTransferenciaPorEntradaId((long)id);

                    viewModel = new EstoqueTransferenciaProdutoViewModel
                    {
                        EstoqueSaidaId = (long)output.PreMovimentoSaida.EstoqueId,
                        EstoqueEntradaId = (long)output.PreMovimentoEntrada.EstoqueId,
                        Documento = output.Documento,
                        PreMovimentoSaidaId = output.PreMovimentoSaidaId,
                        PreMovimentoEntradaId = output.PreMovimentoEntradaId,
                        Id = output.Id,
                        EstoqueEntrada = output.PreMovimentoEntrada.Estoque?.Descricao,
                        EstoqueSaida = output.PreMovimentoSaida.Estoque?.Descricao
                    };
                }
            }
            else //Novo
            {
                viewModel = new EstoqueTransferenciaProdutoViewModel();
            }

            viewModel.Movimento = DateTime.Now;

            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/ConfirmacaoMovimentos/_TransferenciaModal.cshtml", viewModel);
        }

        public async Task<ActionResult> DevolucaoModal(long? id)
        {
            using (var abpSession = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
            using (var preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            {
                var userId = abpSession.Object.UserId.Value;

                CriarOuEditarPreMovimentoModalViewModel viewModel;

                if (id.HasValue) //edição
                {
                    EstoquePreMovimentoDto output = await preMovimentoAppService.Object.Obter((long)id);

                    viewModel = new CriarOuEditarPreMovimentoModalViewModel(output)
                    {
                        PermiteConfirmacaoEntrada = await preMovimentoAppService.Object.PermiteConfirmarEntrada(output)
                    };
                }
                else //Novo
                {
                    viewModel = new CriarOuEditarPreMovimentoModalViewModel(new PreMovimentoViewModel());
                }

                viewModel.Movimento = DateTime.Now;
                return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/ConfirmacaoMovimentos/_DevolucaoModal.cshtml", viewModel);
            }
        }
    }
}