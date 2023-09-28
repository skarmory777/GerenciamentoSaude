using Abp.Runtime.Session;

using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cfops;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Relatorios;

using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class RelatorioEntradaController : Controller // Web.Controllers.SWMANAGERControllerBase
    {
        private readonly IEstoquePreMovimentoAppService _preMovimentoAppService;
        private readonly IFornecedorAppService _fornecedorAppService;
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
        private readonly IPacienteAppService _pacienteAppService;
        private readonly IEstoquePreMovimentoLoteValidadeAppService _estoquePreMovimentoLoteValidadeAppService;
        private readonly IProdutoLaboratorioAppService _produtoLaboratorioAppService;
        private readonly IEstMovimentoBaixaAppService _estMovimentoBaixaAppService;


        public RelatorioEntradaController(
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
            IEstMovimentoBaixaAppService estMovimentoBaixaAppService
            )
        {
            _preMovimentoAppService = preMovimentacaoAppService;
            _userAppService = userAppService;
            AbpSession = abpSession;
            _fornecedorAppService = fornecedorAppService;
            _tipoMovimentoAppService = tipoMovimentoAppService;
            _produtoEstoqueAppService = produtoEstoqueAppService;
            _produtoAppService = produtoAppService;
            _estoquePreMovimentoItemAppService = estoquePreMovimentoItemAppService;
            _estoqueLoteValidadeAppService = estoqueLoteValidadeAppService;
            _produtoLaboratorioAppService = produtoLaboratorioAppService;
            _ordemCompraAppService = ordemCompraAppService;
            _CFOPAppService = CFOPAppService;
            _TipoFreteAppService = TipoFreteAppService;
            _unidadeAppService = unidadeAppService;
            _centroCustoAppService = centroCustoAppService;
            _pacienteAppService = pacienteAppService;
            _estoquePreMovimentoLoteValidadeAppService = estoquePreMovimentoLoteValidadeAppService;
            _estMovimentoBaixaAppService = estMovimentoBaixaAppService;
        }

        public async Task<ActionResult> Index(long preMovimentoId)
        {
            var movimentoRelatorio = new RelatorioEntradaModelDto { PreMovimentoId = preMovimentoId };

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/IndexEntrada.cshtml", new RelatorioEntradaModel(movimentoRelatorio));
        }

        public async Task<ActionResult> Visualizar(long id)
        {

            var movimentoRelatorio = _preMovimentoAppService.ObterDadosRelatorioEntrada(id);

            if (movimentoRelatorio.IsEntrada)
            {
                return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/Entrada.aspx", new RelatorioEntradaModel(movimentoRelatorio));
            }
            else
            {
                return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/Saida.aspx", new RelatorioEntradaModel(movimentoRelatorio));
            }
        }
    }
}