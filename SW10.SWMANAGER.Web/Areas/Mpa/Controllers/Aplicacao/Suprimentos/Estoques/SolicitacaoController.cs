using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class SolicitacaoController : Controller
    {
        public async Task<ActionResult> Index()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Solicitacoes/Index.cshtml", new PreMovimentoViewModel());
        }


        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            using (var estoquePreMovimentoItemAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoItemAppService>())
            using (var preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            using (var abpSession = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
            {
                var userId = abpSession.Object.UserId.Value;
                CriarOuEditarPreMovimentoModalViewModel viewModel = new CriarOuEditarPreMovimentoModalViewModel(new PreMovimentoViewModel());

                if (id != null && id.HasValue) //edição
                {
                    EstoquePreMovimentoDto output = await preMovimentoAppService.Object.Obter((long)id).ConfigureAwait(false);

                    viewModel = new CriarOuEditarPreMovimentoModalViewModel(output)
                    {
                        PermiteConfirmacaoEntrada = await preMovimentoAppService.Object.PermiteConfirmarEntrada(output).ConfigureAwait(false)
                    };

                    var itens = estoquePreMovimentoItemAppService.Object.ObterItensSolicitacaoPorPreMovimento((long)id);
                    viewModel.Itens = JsonConvert.SerializeObject(itens);
                }

                viewModel.Movimento = DateTime.Now;
                return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Solicitacoes/_CriarOuEditarModal.cshtml", viewModel);
            }
        }

        public async Task<ActionResult> CriarOuEditarPreMovimentoItemModal(string item)
        {
            CriarOuEditarPreMovimentoItemModalViewModel viewModel;
            // var produtos = await _produtoAppService.ListarTodosParaMovimento();
            EstoquePreMovimentoItemSolicitacaoDto preMovimentoItem = new EstoquePreMovimentoItemSolicitacaoDto();

            if (!string.IsNullOrEmpty(item))
            {
                preMovimentoItem = JsonConvert.DeserializeObject<EstoquePreMovimentoItemSolicitacaoDto>(item);
            }


            if (preMovimentoItem.IdGrid != null && preMovimentoItem.IdGrid != 0)
            {
                viewModel = new CriarOuEditarPreMovimentoItemModalViewModel(new EstoquePreMovimentoItemDto
                {
                    ProdutoId = preMovimentoItem.ProdutoId,
                    ProdutoUnidadeId = preMovimentoItem.ProdutoUnidadeId
                })
                {
                    Produto = new ProdutoDto { Id = preMovimentoItem.ProdutoId, Descricao = preMovimentoItem.Produto, Codigo = preMovimentoItem.CodigoProduto },
                    // var unidades = await _produtoAppService.ObterUnidadePorProduto(model.ProdutoId);
                    // viewModel.Unidades = new SelectList(unidades.Items, "Id", "Descricao", model.ProdutoUnidadeId);
                    Quantidade = preMovimentoItem.Quantidade,
                    NumeroSerie = preMovimentoItem.NumeroSerie,
                    IdGrid = preMovimentoItem.IdGrid
                };
            }
            else
            {
                viewModel = new CriarOuEditarPreMovimentoItemModalViewModel(new EstoquePreMovimentoItemDto())
                {
                    // viewModel.Produtos = new SelectList(produtos.Items, "Id", "Descricao");
                    Unidades = new SelectList(new ListResultDto<Unidade>().Items, "Id", "Descricao"),
                    LotesValidades = new SelectList(new ListResultDto<LoteValidadeDto>().Items, "Id", "Nome")
                };
            }
            viewModel.Id = preMovimentoItem.Id;

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Solicitacoes/_CriarOuEditarPreMovimentoItemModal.cshtml", viewModel);
        }

        public async Task<ActionResult> CriarOuEditarPreMovimentoKitEstoqueItemModal()
        {
            CriarOuEditarPreMovimentoKitEstoqueItemModalViewModel viewModel;
            using (var kitEstoqueAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueKitAppService>())
            {
                var kitsEstoque = await kitEstoqueAppService.Object.ListarTodos().ConfigureAwait(false);

                viewModel = new CriarOuEditarPreMovimentoKitEstoqueItemModalViewModel();
                viewModel.KitsEstoque = new SelectList(kitsEstoque.Items, "Id", "Descricao");
                viewModel.Quantidade = 1;

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Solicitacoes/_CriarOuEditarPreMovimentoKitEstoqueItemModal.cshtml", viewModel);
            }
        }

        public ActionResult SelecionarSolicitacaoPorPrescricaoModal()
        {
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Solicitacoes/_SelecionarSolicitacaoPorPrescricaoModal.cshtml", null);
        }

        [UnitOfWork(false)]
        public ActionResult ImprimirSolicitacao(long preMovimentoId)
        {
            using (var estoquePreMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            {
                return this.File(estoquePreMovimentoAppService.Object.RetornaArquivoSolicitacao(preMovimentoId), "application/pdf", $"solicitacao-{preMovimentoId}.pdf");
            }
        }
    }
}