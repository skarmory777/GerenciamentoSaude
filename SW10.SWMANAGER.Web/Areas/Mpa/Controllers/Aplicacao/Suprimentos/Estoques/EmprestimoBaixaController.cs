using Abp.Dependency;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pessoas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Suprimentos.Estoques
{
    public class EmprestimoBaixaController : SWMANAGERControllerBase
    {
        [HttpGet]
        public ActionResult Recebimento()
        {
            var viewModel = new PreMovimentoViewModel()
            {
                EstTipoOperacaoId = (long)EnumTipoOperacao.Entrada,
                EstTipoMovimentoId = (long)EnumTipoMovimento.Emprestimo_Entrada
            };

            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Emprestimos/Baixa/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Concessao()
        {
            var viewModel = new PreMovimentoViewModel()
            {
                EstTipoOperacaoId = (long)EnumTipoOperacao.Saida,
                EstTipoMovimentoId = (long)EnumTipoMovimento.Emprestimo_Saida
            };

            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Emprestimos/Baixa/Index.cshtml", viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> AbrirSolicitacao(long id)
        {
            CriarOuEditarPreMovimentoModalViewModel viewModel;
            EstoquePreMovimentoDto output;

            using (var produtoEstoqueAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueAppService>())
            using (var tipoMovimentoAppService = IocManager.Instance.ResolveAsDisposable<ITipoMovimentoAppService>())
            using (var tipoOperacoesAppService = IocManager.Instance.ResolveAsDisposable<ITipoOperacaoAppService>())
            using (var unidadeOrganizacionalAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeOrganizacionalAppService>())
            using (var preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            using (var estoquePreMovimentoItemAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoItemAppService>())
            using (var emprestimoAppService = IocManager.Instance.ResolveAsDisposable<IEmprestimoAppService>())
            {
                output = await emprestimoAppService.Object.ObterSolicitacaoParaBaixa(id).ConfigureAwait(false);

                var empresas = new List<SisPessoaDto>
                {
                    output.EstoqueEmprestimo.SisPessoa
                };

                var estoques = await produtoEstoqueAppService.Object.ListarTodos().ConfigureAwait(false);
                var tipomovimentos = await tipoMovimentoAppService.Object.ListarTodos().ConfigureAwait(false);
                var unidadesOrganizacionais = await unidadeOrganizacionalAppService.Object.ListarTodos().ConfigureAwait(false);
                var tipoOperacoes = await tipoOperacoesAppService.Object.Listar().ConfigureAwait(false);

                viewModel = new CriarOuEditarPreMovimentoModalViewModel(output)
                {
                    Empresas = new SelectList(empresas, "Id", "NomeFantasia", output.EstoqueEmprestimo.SisPessoaId),
                    UnidadesOrganizacionais = new SelectList(unidadesOrganizacionais.Items, "Id", "Descricao", output.UnidadeOrganizacionalId),
                    TipoOperacaoes = new SelectList(tipoOperacoes.Items, "Id", "Descricao", output.EstTipoOperacaoId),
                    TipoMovimentos = new SelectList(tipomovimentos.Items, "Id", "Descricao", output.EstTipoMovimentoId),
                    Estoques = new SelectList(estoques.Items, "Id", "Descricao", output.EstoqueId),
                    PermiteConfirmacaoEntrada = await preMovimentoAppService.Object.PermiteConfirmarEntrada(output).ConfigureAwait(false)
                };
                var itens = estoquePreMovimentoItemAppService.Object.ObterItensSolicitacaoPorPreMovimento(id);
                viewModel.Itens = JsonConvert.SerializeObject(itens);

                viewModel.Movimento = DateTime.Now;

                return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Emprestimos/Baixa/CriarOuEditarModal/_CriarOuEditarModal.cshtml", viewModel);
            }
        }


        [HttpPost]
        public async Task<ActionResult> CriarOuEditarPreMovimentoItemModal(string item)
        {
            CriarOuEditarPreMovimentoItemModalViewModel viewModel;
            var solicitacaoItem = new EstoquePreMovimentoItemSolicitacaoDto();

            using (var preMovimentoItemService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoItemAppService>())
            using (var produtoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoAppService>())
            using (var unidadeAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeAppService>())
            {
                if (!string.IsNullOrEmpty(item))
                {
                    solicitacaoItem = JsonConvert.DeserializeObject<EstoquePreMovimentoItemSolicitacaoDto>(item);
                }

                var model = new EstoquePreMovimentoItemDto
                {
                    ProdutoId = solicitacaoItem.ProdutoId,
                    ProdutoUnidadeId = solicitacaoItem.ProdutoUnidadeId,
                    QuantidadeAtendida = solicitacaoItem.QuantidadeAtendida,
                    PreMovimentoItemEstadoId = solicitacaoItem.EstadoSolicitacaoItemId
                };

                viewModel = new CriarOuEditarPreMovimentoItemModalViewModel(model);

                var produto = await produtoAppService.Object.Obter(solicitacaoItem.ProdutoId).ConfigureAwait(false);
                viewModel.Produto = new ProdutoDto
                {
                    Id = solicitacaoItem.ProdutoId,
                    Descricao = solicitacaoItem.Produto,
                    Codigo = solicitacaoItem.CodigoProduto,
                    IsLote = produto.IsLote,
                    IsValidade = produto.IsValidade,
                    IsSerie = produto.IsSerie
                };
                
                var unidade = await unidadeAppService.Object.Obter((long)solicitacaoItem.ProdutoUnidadeId).ConfigureAwait(false);
                viewModel.ProdutoUnidade = unidade;
                viewModel.Quantidade = solicitacaoItem.Quantidade;
                viewModel.QuantidadeSolicitada = solicitacaoItem.QuantidadeSolicitada;
                viewModel.IdGrid = solicitacaoItem.IdGrid;
                viewModel.LotesValidadesJson = solicitacaoItem.LotesValidadesJson;
                viewModel.NumerosSerieJson = solicitacaoItem.NumerosSerieJson;
                viewModel.Id = solicitacaoItem.Id;
                viewModel.PreMovimentoId = solicitacaoItem.PreMovimentoId;

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Emprestimos/Baixa/CriarOuEditarItemModal/_CriarOuEditarPreMovimentoItemModal.cshtml", viewModel);
            }
        }
    }
}