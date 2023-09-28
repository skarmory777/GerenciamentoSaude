#region Usings
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasApresentacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasLaboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasPrecos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasPrecos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.BrasPrecos;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;
#endregion usings.

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class FaturamentoBrasPrecosController : SWMANAGERControllerBase
    {
        #region Cabecalho

        private readonly IFaturamentoBrasPrecoAppService _brasPrecoAppService;
        private readonly IFaturamentoBrasItemAppService _faturamentoBrasItemAppService;
        private readonly IFaturamentoBrasApresentacaoAppService _faturamentoBrasApresentacaoAppService;
        private readonly IFaturamentoBrasLaboratorioAppService _faturamentoBrasLaboratorioAppService;

        public FaturamentoBrasPrecosController(
            IFaturamentoBrasPrecoAppService brasPrecoAppService,
            IFaturamentoBrasItemAppService faturamentoBrasItemAppService,
            IFaturamentoBrasApresentacaoAppService faturamentoBrasApresentacaoAppService,
            IFaturamentoBrasLaboratorioAppService faturamentoBrasLaboratorioAppService
            )
        {
            _brasPrecoAppService = brasPrecoAppService;
            _faturamentoBrasItemAppService = faturamentoBrasItemAppService;
            _faturamentoBrasLaboratorioAppService = faturamentoBrasLaboratorioAppService;
            _faturamentoBrasApresentacaoAppService = faturamentoBrasApresentacaoAppService;
        }

        #endregion cabecalho.

        public async Task<ActionResult> Index()
        {
            var model = new FaturamentoBrasPrecosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/BrasPrecos/Index.cshtml", model);
        }

        //    [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasPreco_Create, AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasPreco_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarFaturamentoBrasPrecoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _brasPrecoAppService.Obter((long)id);
                viewModel = new CriarOuEditarFaturamentoBrasPrecoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarFaturamentoBrasPrecoModalViewModel(new FaturamentoBrasPrecoDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/BrasPrecos/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}