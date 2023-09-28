using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Financeiros;
using SW10.SWMANAGER.Web.Controllers;

using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class FormaPagamentosController : SWMANAGERControllerBase
    {
        private readonly IFormaPagamentoAppService _formaPagamentoAppService;

        public FormaPagamentosController(IFormaPagamentoAppService formaPagamentoAppService)
        {
            _formaPagamentoAppService = formaPagamentoAppService;
        }



        public ActionResult Index()
        {
            var model = new FormaPagamentoViewModel(new FormaPagamentoDto());
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/FormaPagamento/Index.cshtml", model);
        }

        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            FormaPagamentoViewModel viewModel = null;
            if (id == null || id == 0)
            {
                viewModel = new FormaPagamentoViewModel(new FormaPagamentoDto());
            }
            else
            {
                var formaPagamentoDto = await _formaPagamentoAppService.Obter((long)id);

                viewModel = new FormaPagamentoViewModel(formaPagamentoDto);
            }


            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/FormaPagamento/_CriarOuEditarModal.cshtml", viewModel);
        }


    }
}