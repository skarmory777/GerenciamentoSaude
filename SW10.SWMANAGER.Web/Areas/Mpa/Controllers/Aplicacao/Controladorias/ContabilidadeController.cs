using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Controladorias.Contabilidade;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Controladorias
{
    public class ContabilidadeController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new ContabilidadeViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Controladorias/Contabilidade/Index.cshtml", viewModel);
        }
    }
}