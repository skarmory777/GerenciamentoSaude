using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Interface;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Financeiros.Bancarios.ContaTesouraria;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Financeiros.Bancarios.ContaTesouraria
{
    public class ContaCorrenteController : SWMANAGERControllerBase
    {

        private readonly IContaCorrenteAppService _contaCorrenteAppService;

        public ContaCorrenteController(IContaCorrenteAppService contaCorrenteAppService)
        {
            _contaCorrenteAppService = contaCorrenteAppService;
        }

        // GET: Mpa/TipoLocalChamada
        public ActionResult Index()
        {
            var model = new ContaCorrenteViewModel(new ContaCorrenteDto());
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/Bancarios/ContaTesouraria/Index.cshtml", model);
        }

        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            ContaCorrenteViewModel viewModel = null;
            if (id == null || id == 0)
            {
                viewModel = new ContaCorrenteViewModel(new ContaCorrenteDto());
            }
            else
            {
                var contaCorrenteDto = await _contaCorrenteAppService.Obter((long)id);

                viewModel = new ContaCorrenteViewModel(contaCorrenteDto);
            }



            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/Bancarios/ContaTesouraria/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}