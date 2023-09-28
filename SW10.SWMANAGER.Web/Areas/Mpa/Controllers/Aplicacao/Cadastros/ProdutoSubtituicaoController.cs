//using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEspecie;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{

    public class ProdutosSubstituicaoController : Controller // SWMANAGERControllerBase
    {

        //private readonly IProdutoRelacaoAcaoTerapeuticaAppService _produtoRelacaoAcaoTerapeuticaAppService;
        //private readonly IProdutoRelacaoPortariaAppService _produtoRelacaoPortariaAppService;
        //private readonly IProdutoRelacaoLaboratorioAppService _produtoRelacaoLaboratorioAppService;
        //private readonly IProdutoRelacaoPalavraChaveAppService _produtoRelacaoPalavraChaveAppService;
        //private readonly IProdutoAppService _produtoAppService;
        //private readonly ISexoAppService _sexoAppService;
        //private readonly IProdutoUnidadeAppService _produtoUnidadeAppService;
        //private readonly IProdutoUnidadeTipoAppService _produtoUnidadeTipoAppService;
        //private readonly IProdutoEspecieAppService _produtoEspecieAppService;
        //private readonly IProdutoClasseAppService _produtoClasseAppAppService;
        //private readonly IProdutoSubClasseAppService _produtoSubClasseAppService;
        //private readonly IProdutoCodigoMedicamentoAppService _produtoCodigoMedicamentosAppService;
        //private readonly IProdutoListaSubstituicaoAppService _produtoListaSubstituicaoAppService;
        //private readonly IProdutoRelacaoEstoqueAppService _produtoRelacaoEstoqueAppService;
        //private readonly IUnidadeAppService _unidadeAppService;

        //public ProdutosSubstituicaoController(
        //    //IProdutoRelacaoEstoqueAppService produtoRelacaoEstoqueAppService,
        //    //IProdutoAppService produtoAppService,
        //    //IProdutoRelacaoAcaoTerapeuticaAppService produtoRelacaoAcaoTerapeuticaAppService,
        //    //IProdutoRelacaoPortariaAppService produtoRelacaoPortariaAppService,
        //    //IProdutoRelacaoLaboratorioAppService produtoRelacaoLaboratorioAppService,
        //    //IProdutoRelacaoPalavraChaveAppService produtoRelacaoPalavraChaveAppService,
        //    //ISexoAppService sexoAppService,
        //    //IProdutoUnidadeAppService produtoUnidadeAppService,
        //    //IProdutoUnidadeTipoAppService produtoUnidadeTipoAppService,
        //    //IProdutoEspecieAppService produtoEspecieAppService,
        //    //IProdutoClasseAppService produtoClasseAppAppService,
        //    //IProdutoSubClasseAppService produtoSubClasseAppService,
        //    //IProdutoCodigoMedicamentoAppService produtoCodigoMedicamentosAppService,
        //    //IProdutoListaSubstituicaoAppService produtoListaSubstituicaoAppService,
        //    //IUnidadeAppService unidadeAppService
        //    )
        //{
        //    //_produtoRelacaoEstoqueAppService = produtoRelacaoEstoqueAppService;
        //    //_produtoAppService = produtoAppService;
        //    //_produtoRelacaoAcaoTerapeuticaAppService = produtoRelacaoAcaoTerapeuticaAppService;
        //    //_produtoRelacaoPortariaAppService = produtoRelacaoPortariaAppService;
        //    //_produtoRelacaoLaboratorioAppService = produtoRelacaoLaboratorioAppService;
        //    //_produtoRelacaoPalavraChaveAppService = produtoRelacaoPalavraChaveAppService;
        //    //_sexoAppService = sexoAppService;
        //    //_produtoUnidadeAppService = produtoUnidadeAppService;
        //    //_produtoUnidadeTipoAppService = produtoUnidadeTipoAppService;
        //    //_produtoEspecieAppService = produtoEspecieAppService;
        //    //_produtoClasseAppAppService = produtoClasseAppAppService;
        //    //_produtoSubClasseAppService = produtoSubClasseAppService;
        //    //_produtoCodigoMedicamentosAppService = produtoCodigoMedicamentosAppService;
        //    //_produtoListaSubstituicaoAppService = produtoListaSubstituicaoAppService;
        //    //_unidadeAppService = unidadeAppService;
        //}

        ////public ActionResult Index()
        ////{


        ////    var model = new ProdutoSubtituicaoViewModel();
        ////    return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Produtos/_CriarOuEditarProdutoSubtituicaoModal.cshtml", model);
        ////}


        //[AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Suprimentos_Produto_Create, AppPermissions.Pages_Tenant_Cadastros_Suprimentos_Produto_Edit)]
        //public async Task<ActionResult> CriarOuEditarModal(long? id)
        //{
        //    //var sexos = await _sexoAppService.ListarTodos();
        //    //var produtos = await _produtoAppService.ListarProdutosMestre();
        //    //var especies = await _produtoEspecieAppService.ListarTodos();
        //    //var classes = await _produtoClasseAppAppService.ListarTodos();
        //    //var subClasses = await _produtoSubClasseAppService.ListarTodos();
        //    //var codigoMedicamentosAppService = await _produtoCodigoMedicamentosAppService.ListarTodos();
        //    //var Unidades = await _unidadeAppService.ListarTodos();


        //    //CriarOuEditarProdutoModalViewModel viewModel;

        //    //if (id.HasValue)
        //    //{
        //    //    var produtoAtual = produtos.Items.FirstOrDefault(p => p.Id == id);

        //    //    List<ProdutoDto> prods = new List<ProdutoDto>();
        //    //    prods.Add(produtoAtual);
        //    //    var produtosMaster = produtos.Items.Except(prods);

        //    //    var output = await _produtoAppService.Obter((long)id);
        //    //    viewModel = new CriarOuEditarProdutoModalViewModel(output);
        //    //    viewModel.Sexos = new SelectList(sexos.Items, "Id", "Descricao", output.Sexo);
        //    //    viewModel.ProdutosMestre = new SelectList(produtosMaster, "Id", "Descricao", output.ProdutoMestreId);
        //    //    viewModel.Especies = new SelectList(especies.Items, "Id", "Descricao", output.EspecieId);
        //    //    viewModel.Classes = new SelectList(classes.Items, "Id", "Descricao", output.ClasseId);
        //    //    viewModel.SubClasses = new SelectList(subClasses.Items, "Id", "Descricao", output.SubClasseId);
        //    //    viewModel.CodigosMedicamentos = new SelectList(codigoMedicamentosAppService.Items, "Id", "Descricao", output.CodigoMedicamentosId);
        //    //    viewModel.Unidades = new SelectList(Unidades.Items, "Id", "Descricao", output.UnidadeReferenciaId);
        //    //}
        //    //else
        //    //{
        //    //    viewModel = new CriarOuEditarProdutoModalViewModel(new ProdutoDto());
        //    //    viewModel.Sexos = new SelectList(sexos.Items, "Id", "Descricao");
        //    //    viewModel.ProdutosMestre = new SelectList(produtos.Items, "Id", "Descricao");
        //    //    viewModel.Especies = new SelectList(especies.Items, "Id", "Descricao");
        //    //    viewModel.Classes = new SelectList(classes.Items, "Id", "Descricao");
        //    //    viewModel.SubClasses = new SelectList(subClasses.Items, "Id", "Descricao");
        //    //    viewModel.CodigosMedicamentos = new SelectList(codigoMedicamentosAppService.Items, "Id", "Descricao");
        //    //    viewModel.Unidades = new SelectList(Unidades.Items, "Id", "Descricao");
        //    //}

        //    //return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Produtos/_CriarOuEditarModal.cshtml", viewModel);




        //    //var model = new ProdutoListaSubstituicaoModalViewModel();
        //    //return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Produtos/_CriarOuEditarProdutoSubtituicaoModal.cshtml", model);

        //}

    }
}