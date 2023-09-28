using SW10.SWMANAGER.ClassesAplicacao.Services.Desenvolvimento;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Desenvolvimento.DocItens;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Desenvolvimento.DocItens
{
    public class DocItensController : SWMANAGERControllerBase
    {
        private readonly IDocItemAppService _docItemAppService;

        public DocItensController(
            IDocItemAppService docItemAppService
            )
        {
            _docItemAppService = docItemAppService;
        }

        public ActionResult Index()
        {
            var model = new DocItensListagemViewModel();
            //model.DocItem = new ClassesAplicacao.Desenvolvimento.DocItemDto();

            return View("~/Areas/Mpa/Views/Aplicacao/Desenvolvimento/DocItens/Listagem.cshtml", model);
        }

    }
}