using Abp.Runtime.Session;

using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cfops;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class BaixaValesController : Controller
    {
        private readonly ITipoMovimentoAppService _tipoMovimentoAppService;
        private readonly IUserAppService _userAppService;
        private readonly IAbpSession AbpSession;
        private readonly IEstoqueAppService _produtoEstoqueAppService;
        private readonly IProdutoAppService _produtoAppService;
        private readonly IEstoquePreMovimentoItemAppService _estoquePreMovimentoItemAppService;
        private readonly IEstoqueLoteValidadeAppService _estoqueLoteValidadeAppService;
        private readonly IOrdemCompraAppService _ordemCompraAppService;
        private readonly ICfopAppService _CFOPAppService;
        private readonly ITipoFreteAppService _TipoFreteAppService;
        private readonly IUnidadeAppService _unidadeAppService;
        private readonly ICentroCustoAppService _centroCustoAppService;
        private readonly IFornecedorAppService _fornecedorAppService;
        private readonly IPacienteAppService _pacienteAppService;
        private readonly IEstoqueMovimentoAppService _movimentoAppService;

        public BaixaValesController(

             IEstoquePreMovimentoAppService preMovimentacaoAppService,
            IUserAppService userAppService,
            IAbpSession abpSession,
            IFornecedorAppService fornecedorAppService,
           ITipoMovimentoAppService tipoMovimentoAppService,
            IEstoqueAppService produtoEstoqueAppService,
            IProdutoAppService produtoAppService,
            IEstoquePreMovimentoItemAppService estoquePreMovimentoItemAppService,
            IEstoqueLoteValidadeAppService estoqueLoteValidadeAppService,
            IProdutoLaboratorioAppService produtoLaboratorioAppService,
            IOrdemCompraAppService ordemCompraAppService,
            ICfopAppService CFOPAppService,
            ITipoFreteAppService TipoFreteAppService,
            IUnidadeAppService unidadeAppService,
            ICentroCustoAppService centroCustoAppService,
            IPacienteAppService pacienteAppService,
            IEstoquePreMovimentoLoteValidadeAppService estoquePreMovimentoLoteValidadeAppService,
            IEstoqueMovimentoAppService movimentoAppService

            )
        {
            _userAppService = userAppService;
            AbpSession = abpSession;
            _fornecedorAppService = fornecedorAppService;
            _tipoMovimentoAppService = tipoMovimentoAppService;
            _produtoEstoqueAppService = produtoEstoqueAppService;
            _produtoAppService = produtoAppService;
            _estoquePreMovimentoItemAppService = estoquePreMovimentoItemAppService;
            _estoqueLoteValidadeAppService = estoqueLoteValidadeAppService;
            _ordemCompraAppService = ordemCompraAppService;
            _CFOPAppService = CFOPAppService;
            _TipoFreteAppService = TipoFreteAppService;
            _unidadeAppService = unidadeAppService;
            _centroCustoAppService = centroCustoAppService;
            _pacienteAppService = pacienteAppService;
            _movimentoAppService = movimentoAppService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new PreMovimentoViewModel();

            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/BaixaVales/Index.cshtml", model);
        }

        public async Task<ActionResult> CriarOuEditarModal(string id)
        {
            var userId = AbpSession.UserId.Value;
            var userEmpresas = await _userAppService.GetUserEmpresas(userId);
            var tipoMovimnetacoes = await _tipoMovimentoAppService.Listar(true);
            var estoques = await _produtoEstoqueAppService.ListarTodos();
            var ordens = await _ordemCompraAppService.ListarTodos();
            var CFOPs = await _CFOPAppService.ListarTodos();
            var tiposFretes = await _TipoFreteAppService.ListarTodos();
            var centroCustos = await _centroCustoAppService.ListarTodos();
            var pacientes = await _pacienteAppService.ListarTodos();

            MovimentoModalViewModel viewModel;


            viewModel = new MovimentoModalViewModel(new EstoqueMovimentoDto());


            if (userEmpresas.Items.Count == 1)
            {
                var empresaId = userEmpresas.Items.First().Id;
                viewModel.Empresas = new SelectList(userEmpresas.Items, "Id", "NomeFantasia", empresaId);
            }
            else
            {
                viewModel.Empresas = new SelectList(userEmpresas.Items, "Id", "NomeFantasia");
            }


            long var;


            viewModel.TipoMovimentos = new SelectList(tipoMovimnetacoes.Items, "Id", "Descricao");
            viewModel.EstTipoMovimentoId = (long)EnumTipoMovimento.NotaFiscal_Entrada;

            //viewModel.Fornecedores = SelecionarSelectListUnitario<CriarOuEditarFornecedor>(fornecedores.Items, "Id", "Descricao", out var);
            //viewModel.FornecedorId = var;

            //viewModel.TipoDocumentos = SelecionarSelectListUnitario<TipoDocumentoDto>(tipoDocumentos.Items, "Id", "Descricao", out var);
            //viewModel.TipoDocumentoId = var;

            viewModel.Estoques = SelecionarSelectListUnitario<EstoqueDto>(estoques.Items, "Id", "Descricao", out var);
            viewModel.EstoqueId = var;

            viewModel.Ordens = SelecionarSelectListUnitario<OrdemCompraDto>(ordens.Items, "Id", "Descricao", out var);
            viewModel.OrdemId = var;

            viewModel.TipoFretes = SelecionarSelectListUnitario<TipoFreteDto>(tiposFretes.Items, "Id", "Descricao", out var);
            viewModel.TipoFreteId = var;

            viewModel.CentroCustos = SelecionarSelectListUnitario<CentroCustoDto>(centroCustos.Items, "Id", "Descricao", out var);
            viewModel.CentroCustoId = var;

            viewModel.Pacientes = SelecionarSelectListUnitario<PacienteDto>(pacientes.Items, "Id", "NomeCompleto", out var);
            viewModel.PacienteId = var;

            viewModel.Serie = "1";
            viewModel.Movimento = DateTime.Now;

            viewModel.ValesIds = id;

            var movimentoId = id.TrimEnd('-').Split('-').FirstOrDefault();

            if (!string.IsNullOrEmpty(movimentoId))
            {
                var movimento = await _movimentoAppService.Obter(long.Parse(movimentoId));

                if (movimento != null && movimento.FornecedorId != null)
                {
                    var fornecedor = await _fornecedorAppService.Obter((long)movimento.FornecedorId);
                    if (fornecedor != null)
                    {
                        viewModel.Fornecedor = new FornecedorDto { Id = fornecedor.Id, Descricao = fornecedor.Descricao };
                        viewModel.FornecedorId = fornecedor.Id;
                    }
                }
            }

            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/BaixaVales/_CriarOuEditarModal.cshtml", viewModel);
        }


        SelectList SelecionarSelectListUnitario<T>(IReadOnlyList<T> items, string id, string descricao, out long identificador)
        {
            identificador = 0;
            if (items.Count == 1)
            {
                var item = ((List<T>)items)[0];
                var i = item.GetType().GetProperty("Id").GetValue(item);

                long.TryParse(i.ToString(), out identificador);

                return new SelectList(items, id, descricao, identificador);
            }


            return new SelectList(items, id, descricao);
        }


        public async Task<ActionResult> CriarOuEditarMovimentoItemModal(long? id)
        {
            CriarOuEditarMovimentoItemModalViewModel viewModel = null;
            var produtos = await _produtoAppService.ListarTodosParaMovimento().ConfigureAwait(false);

            if (id.HasValue && id.Value != 0)
            {
                var model = await _movimentoAppService.ObterItem(id.Value).ConfigureAwait(false);
                viewModel = new CriarOuEditarMovimentoItemModalViewModel(model);
                if (model != null)
                {
                    viewModel.Produtos = new SelectList(produtos.Items, "Id", "Descricao", model.ProdutoId);
                    var unidades = await _produtoAppService.ObterUnidadePorProduto(model.ProdutoId).ConfigureAwait(false);
                    viewModel.Unidades = new SelectList(unidades.Items, "Id", "Descricao", model.ProdutoUnidadeId);
                    var unidade = await _unidadeAppService.ObterUnidadeDto((long)model.ProdutoUnidadeId).ConfigureAwait(false);
                    if (unidade != null)
                    {
                        viewModel.Quantidade = viewModel.Quantidade / unidade.Fator;
                        viewModel.Unidade = unidade.Descricao;
                    }
                    viewModel.CustoTotal = viewModel.Quantidade * viewModel.CustoUnitario;
                    viewModel.ValorIPI = (viewModel.CustoTotal * viewModel.PerIPI) / 100;
                    viewModel.ValorICMS = (viewModel.CustoTotal * viewModel.PerICMS) / 100;

                    var produto = await _produtoAppService.Obter(model.ProdutoId).ConfigureAwait(false); ;
                    if (produto != null)
                    {
                        viewModel.IsNumeroSerie = produto.IsSerie;
                        viewModel.ProdutoDto = produto;
                    }

                }
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/BaixaVales/_CriarOuEditarPreMovimentoItemModal.cshtml", viewModel);
        }

    }
}