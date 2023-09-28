using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Autorizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Autorizacoes.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos;
using SW10.SWMANAGER.Web.Controllers;

using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Faturamentos
{
    public class FaturamentoAutorizacoesController : SWMANAGERControllerBase
    {
        private readonly IFaturamentoAutorizacaoAppService _authAppService;

        public FaturamentoAutorizacoesController(
           IFaturamentoAutorizacaoAppService authAppService
           )
        {
            _authAppService = authAppService;
        }

        public ActionResult Index()
        {
            var viewModel = new FaturamentoAutorizacoesViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Autorizacoes/Index.cshtml", viewModel);
        }

        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            FaturamentoAutorizacaoDto viewModel;
            if (id.HasValue)
            {
                viewModel = await _authAppService.Obter((long)id);

            }
            else
            {
                viewModel = new FaturamentoAutorizacaoDto();
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Autorizacoes/_CriarOuEditarModal.cshtml", viewModel);
        }
    }
}