using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Financeiros;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class GrupoDREsController : SWMANAGERControllerBase
    {
        private readonly IGrupoDREAppService _grupoDREAppService;

        public GrupoDREsController(IGrupoDREAppService grupoDREAppService)
        {
            _grupoDREAppService = grupoDREAppService;
        }



        public ActionResult Index()
        {
            var model = new GrupoDREViewModel(new GrupoDREDto());
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/GrupoDRE/Index.cshtml", model);
        }

        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            GrupoDREViewModel viewModel = null;
            if (id == null || id == 0)
            {
                viewModel = new GrupoDREViewModel(new GrupoDREDto());
            }
            else
            {
                var grupoDREDto = await _grupoDREAppService.Obter((long)id);

                viewModel = new GrupoDREViewModel(grupoDREDto);
            }


            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/GrupoDRE/_CriarOuEditarModal.cshtml", viewModel);
        }


    }
}