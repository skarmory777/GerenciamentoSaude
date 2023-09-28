using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Financeiro.Bancarios.TipoConta;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Financeiros.Bancarios.TipoConta;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Financeiros.Bancarios.TipoConta
{
    public class TipoContaCorrenteController : SWMANAGERControllerBase
    {

        private readonly ITipoContaCorrenteAppService _tipoContaCorrenteAppService;

        public TipoContaCorrenteController(ITipoContaCorrenteAppService tipoContaCorrenteAppService)
        {
            _tipoContaCorrenteAppService = tipoContaCorrenteAppService;
        }

        // GET: Mpa/TipoLocalChamada
        public ActionResult Index()
        {
            var model = new TipoContaCorrenteViewModel(new TipoContaCorrenteDto());
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/Bancarios/TipoConta/Index.cshtml", model);
        }

        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            TipoContaCorrenteViewModel viewModel = null;
            if (id == null || id == 0)
            {
                viewModel = new TipoContaCorrenteViewModel(new TipoContaCorrenteDto());
            }
            else
            {
                var tipoContaCorrenteDto = await _tipoContaCorrenteAppService.Obter((long)id);

                viewModel = new TipoContaCorrenteViewModel(tipoContaCorrenteDto);
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/Bancarios/TipoConta/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}