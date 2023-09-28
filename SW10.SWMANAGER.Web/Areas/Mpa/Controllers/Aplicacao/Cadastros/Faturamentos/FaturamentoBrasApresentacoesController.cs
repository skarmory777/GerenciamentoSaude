using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasApresentacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasApresentacoes.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.BrasApresentacoes;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class FaturamentoBrasApresentacoesController : SWMANAGERControllerBase
    {
        #region Injecao e Contrutor

        private readonly IFaturamentoBrasApresentacaoAppService _brasApresentacaoAppService;


        public FaturamentoBrasApresentacoesController(
            IFaturamentoBrasApresentacaoAppService brasApresentacaoAppService
            )
        {
            _brasApresentacaoAppService = brasApresentacaoAppService;
        }

        #endregion

        public async Task<ActionResult> Index()
        {
            var model = new FaturamentoBrasApresentacoesViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/BrasApresentacoes/Index.cshtml", model);
        }

        //    [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasApresentacao_Create, AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasApresentacao_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarFaturamentoBrasApresentacaoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _brasApresentacaoAppService.Obter((long)id);
                viewModel = new CriarOuEditarFaturamentoBrasApresentacaoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarFaturamentoBrasApresentacaoModalViewModel(new FaturamentoBrasApresentacaoDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/BrasApresentacoes/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}