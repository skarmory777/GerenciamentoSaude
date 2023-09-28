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
    public class EmprestimoDevolucaoController : SWMANAGERControllerBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = new PreMovimentoViewModel();

            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Emprestimos/ConsultaDevolucao/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Baixa()
        {
            var viewModel = new PreMovimentoViewModel();

            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Emprestimos/BaixaDevolucao/Index.cshtml", viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> AbrirSolicitacaoBaixa(long id)
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

                return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Emprestimos/BaixaDevolucao/CriarOuEditarModal/_CriarOuEditarModal.cshtml", viewModel);
            }
        }

        [HttpGet]
        public async Task<ActionResult> CriarOuEditarModal(long? id, long? estTipoOperacaoId = (long)EnumTipoOperacao.Entrada)
        {
            var viewModel = new CriarOuEditarPreMovimentoModalViewModel(new PreMovimentoViewModel())
            {
                Movimento = DateTime.Now,
                EstoqueEmprestimo = new EstoqueEmprestimoDto(),
                EstTipoOperacaoId = estTipoOperacaoId
            };

            if (id.HasValue)
            {
                using (var estoquePreMovimentoItemAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoItemAppService>())
                using (var preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
                {
                    var output = await preMovimentoAppService.Object.Obter(id.Value).ConfigureAwait(false);
                    var itens = await estoquePreMovimentoItemAppService.Object.ObterItensPorPreMovimento(id.Value).ConfigureAwait(false);

                    viewModel = new CriarOuEditarPreMovimentoModalViewModel(output)
                    {
                        PermiteConfirmacaoEntrada = await preMovimentoAppService.Object.PermiteConfirmarEntrada(output).ConfigureAwait(false),
                        Itens = JsonConvert.SerializeObject(itens)
                    };
                }
            }

            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Emprestimos/ConsultaDevolucao/CriarOuEditarModal/_CriarOuEditarModal.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> CriarOuEditarPreMovimentoItemModal(string item)
        {
            CriarOuEditarPreMovimentoItemModalViewModel viewModel;
            var preMovimentoItem = new EstoquePreMovimentoItemSolicitacaoDto();

            using (var produtoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoAppService>())
            using (var unidadeAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeAppService>())
            {
                if (!string.IsNullOrEmpty(item))
                {
                    preMovimentoItem = JsonConvert.DeserializeObject<EstoquePreMovimentoItemSolicitacaoDto>(item);
                }

                var model = new EstoquePreMovimentoItemDto
                {
                    ProdutoId = preMovimentoItem.ProdutoId,
                    ProdutoUnidadeId = preMovimentoItem.ProdutoUnidadeId,
                    QuantidadeAtendida = preMovimentoItem.QuantidadeAtendida,
                    PreMovimentoItemEstadoId = preMovimentoItem.EstadoSolicitacaoItemId
                };

                viewModel = new CriarOuEditarPreMovimentoItemModalViewModel(model);

                var produto = await produtoAppService.Object.Obter(preMovimentoItem.ProdutoId).ConfigureAwait(false);
                viewModel.Produto = new ProdutoDto
                {
                    Id = preMovimentoItem.ProdutoId,
                    Descricao = preMovimentoItem.Produto,
                    Codigo = preMovimentoItem.CodigoProduto,
                    IsLote = produto.IsLote,
                    IsValidade = produto.IsValidade,
                    IsSerie = produto.IsSerie
                };

                var unidade = await unidadeAppService.Object.Obter((long)preMovimentoItem.ProdutoUnidadeId).ConfigureAwait(false);
                viewModel.ProdutoUnidade = unidade;
                viewModel.Quantidade = preMovimentoItem.Quantidade;
                viewModel.QuantidadeSolicitada = preMovimentoItem.QuantidadeSolicitada;
                viewModel.IdGrid = preMovimentoItem.IdGrid;
                viewModel.LotesValidadesJson = preMovimentoItem.LotesValidadesJson;
                viewModel.NumerosSerieJson = preMovimentoItem.NumerosSerieJson;
                viewModel.Id = preMovimentoItem.Id;

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Emprestimos/BaixaDevolucao/CriarOuEditarItemModal/_CriarOuEditarPreMovimentoItemModal.cshtml", viewModel);
            }
        }
    }
}