using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Financeiros;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class SituacaoLancamentosController : SWMANAGERControllerBase
    {
        private readonly ISituacaoLancamentoAppService _situacaoLancamentoAppService;

        public SituacaoLancamentosController(ISituacaoLancamentoAppService situacaoLancamentoAppService)
        {
            _situacaoLancamentoAppService = situacaoLancamentoAppService;
        }



        public ActionResult Index()
        {
            var model = new SituacaoLancamentoViewModel(new SituacaoLancamentoDto());
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/SituacaoLancamento/Index.cshtml", model);
        }

        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            SituacaoLancamentoViewModel viewModel = null;
            if (id == null || id == 0)
            {
                viewModel = new SituacaoLancamentoViewModel(new SituacaoLancamentoDto());
            }
            else
            {
                var situacaoLancamentoDto = await _situacaoLancamentoAppService.Obter((long)id);

                viewModel = new SituacaoLancamentoViewModel(situacaoLancamentoDto);
            }


            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/SituacaoLancamento/_CriarOuEditarModal.cshtml", viewModel);
        }


    }
}