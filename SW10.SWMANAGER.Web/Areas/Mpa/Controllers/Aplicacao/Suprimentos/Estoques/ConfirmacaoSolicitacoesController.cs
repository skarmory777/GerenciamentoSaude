using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Uow;
using Newtonsoft.Json;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Relatorios;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Suprimentos.Estoques
{
    public class ConfirmacaoSolicitacoesController : SWMANAGERControllerBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = new PreMovimentoViewModel();

            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/ConfirmacaoSolicitacoes/Index.cshtml", model);
        }

        [HttpGet]
        public async Task<ActionResult> ConfirmarSolicitacao(long? id)
        {
            CriarOuEditarPreMovimentoModalViewModel viewModel = null;
            EstoquePreMovimentoDto output;

            if (!id.HasValue) //edição
                return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/ConfirmacaoSolicitacoes/_CriarOuEditarModal.cshtml", viewModel);

            using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
            using (var produtoEstoqueAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueAppService>())
            using (var tipoMovimentoAppService = IocManager.Instance.ResolveAsDisposable<ITipoMovimentoAppService>())
            using (var motivoPerdaProdutoAppService = IocManager.Instance.ResolveAsDisposable<IMotivoPerdaProdutoAppService>())
            using (var tipoOperacoesAppService = IocManager.Instance.ResolveAsDisposable<ITipoOperacaoAppService>())
            using (var unidadeOrganizacionalAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeOrganizacionalAppService>())
            using (var preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            using (var estoquePreMovimentoItemAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoItemAppService>())
            {
                output = await preMovimentoAppService.Object.ObterParaConfirmarSolicitacao(id.Value).ConfigureAwait(false);

                var userId = AbpSession.UserId.Value;
                var userEmpresas = await userAppService.Object.GetUserEmpresas(userId).ConfigureAwait(false);
                var estoques = await produtoEstoqueAppService.Object.ListarTodos().ConfigureAwait(false);
                var tipomovimentos = await tipoMovimentoAppService.Object.ListarTodos().ConfigureAwait(false);
                var unidadesOrganizacionais = await unidadeOrganizacionalAppService.Object.ListarTodos().ConfigureAwait(false);
                var tipoOperacoes = await tipoOperacoesAppService.Object.Listar().ConfigureAwait(false);
                var motivoPerdaProdutos = await motivoPerdaProdutoAppService.Object.ListarTodos().ConfigureAwait(false);

                viewModel = new CriarOuEditarPreMovimentoModalViewModel(output)
                {
                    Empresas = new SelectList(userEmpresas.Items, "Id", "NomeFantasia", output.EmpresaId),
                    UnidadesOrganizacionais = new SelectList(unidadesOrganizacionais.Items, "Id", "Descricao", output.UnidadeOrganizacionalId),
                    TipoOperacaoes = new SelectList(tipoOperacoes.Items, "Id", "Descricao", output.EstTipoOperacaoId),
                    TipoMovimentos = new SelectList(tipomovimentos.Items, "Id", "Descricao", output.EstTipoMovimentoId),
                    Estoques = new SelectList(estoques.Items, "Id", "Descricao", output.EstoqueId),
                    MotivosPerda = new SelectList(motivoPerdaProdutos.Items, "Id", "Descricao", output.MotivoPerdaProdutoId),
                    PermiteConfirmacaoEntrada = await preMovimentoAppService.Object.PermiteConfirmarEntrada(output).ConfigureAwait(false)
                };

                var itens = estoquePreMovimentoItemAppService.Object.ObterItensSolicitacaoPorPreMovimento((long)id);
                viewModel.Itens = JsonConvert.SerializeObject(itens);

                viewModel.Movimento = DateTime.Now;

                return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/ConfirmacaoSolicitacoes/_CriarOuEditarModal.cshtml", viewModel);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CriarOuEditarPreMovimentoItemModal(string item)
        {
            CriarOuEditarPreMovimentoItemModalViewModel viewModel;
            var preMovimentoItem = new EstoquePreMovimentoItemSolicitacaoDto();

            using (var produtoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoAppService>())
            using (var produtoLaboratorioAppService = IocManager.Instance.ResolveAsDisposable<IProdutoLaboratorioAppService>())
            using (var unidadeAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeAppService>())
            {
                if (!string.IsNullOrEmpty(item))
                {
                    preMovimentoItem = JsonConvert.DeserializeObject<EstoquePreMovimentoItemSolicitacaoDto>(item);
                }

                if (!preMovimentoItem.IdGrid.HasValue || preMovimentoItem.IdGrid == 0)
                {
                    viewModel = new CriarOuEditarPreMovimentoItemModalViewModel(new EstoquePreMovimentoItemDto())
                    {
                        Id = preMovimentoItem.Id,
                        Unidades = new SelectList(new ListResultDto<Unidade>().Items, "Id", "Descricao"),
                        LotesValidades = new SelectList(new ListResultDto<LoteValidadeDto>().Items, "Id", "Nome")
                    };

                    return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/ConfirmacaoSolicitacoes/_CriarOuEditarPreMovimentoItemModal.cshtml", viewModel);
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

                viewModel.Laboratorios = new SelectList(new ListResultDto<ProdutoLaboratorioDto>().Items, "Id", "Descricao");

                if (produto.IsValidade || produto.IsLote)
                {
                    var laboratorios = await produtoLaboratorioAppService.Object.ListarTodos().ConfigureAwait(false);
                    viewModel.Laboratorios = new SelectList(laboratorios.Items, "Id", "Descricao");
                }                

                var unidade = await unidadeAppService.Object.Obter((long)preMovimentoItem.ProdutoUnidadeId).ConfigureAwait(false);
                viewModel.ProdutoUnidade = unidade;
                viewModel.Quantidade = preMovimentoItem.Quantidade;
                viewModel.QuantidadeSolicitada = preMovimentoItem.QuantidadeSolicitada;
                viewModel.IdGrid = preMovimentoItem.IdGrid;
                viewModel.LotesValidadesJson = preMovimentoItem.LotesValidadesJson;
                viewModel.NumerosSerieJson = preMovimentoItem.NumerosSerieJson;
                viewModel.Id = preMovimentoItem.Id;

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/ConfirmacaoSolicitacoes/_CriarOuEditarPreMovimentoItemModal.cshtml", viewModel);
            }
        }


        [AcceptVerbs("GET", "POST", "PUT")]
        public ActionResult Visualizar(long id)
        {
            using (var preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            {
                var movimentoRelatorio = preMovimentoAppService.Object.ObterDadosRelatorioSolicitacao(id);

                return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/Solicitacao.aspx", new RelatorioSolicitacaoModel(movimentoRelatorio));
            }
        }


        public ActionResult VisualizarIndex(long solicitacaoId)
        {
            var movimentoRelatorio = new RelatorioSolicitacaoSaidaModelDto { PreMovimentoId = solicitacaoId };

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/IndexSolicitacao.cshtml", new RelatorioSolicitacaoModel(movimentoRelatorio));
        }

        [UnitOfWork(false)]
        public ActionResult ImprimirSolicitacaoBaixa(long preMovimentoId)
        {
            using (var estoquePreMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            {
                return this.File(estoquePreMovimentoAppService.Object.RetornaArquivoSolicitacaoBaixa(preMovimentoId), "application/pdf", $"solicitacaoBaixa-{preMovimentoId}.pdf");
            }
        }


    }
}