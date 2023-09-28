using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Financeiros;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class MeiosPagamentosController : SWMANAGERControllerBase
    {
        private readonly IMeioPagamentoAppService _meioPagamentoAppService;

        public MeiosPagamentosController(IMeioPagamentoAppService meioPagamentoAppService)
        {
            _meioPagamentoAppService = meioPagamentoAppService;
        }



        public ActionResult Index()
        {
            var model = new MeioPagamentoViewModel(new MeioPagamentoDto());
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/MeioPagamento/Index.cshtml", model);
        }

        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            MeioPagamentoViewModel viewModel = null;
            if (id == null || id == 0)
            {
                viewModel = new MeioPagamentoViewModel(new MeioPagamentoDto());
            }
            else
            {
                var meioPagamentoDto = await _meioPagamentoAppService.Obter((long)id);

                viewModel = new MeioPagamentoViewModel(meioPagamentoDto);
            }


            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/MeioPagamento/_CriarOuEditarModal.cshtml", viewModel);
        }


    }
}