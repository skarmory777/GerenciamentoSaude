using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Controladorias.Patrimonio;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Controladorias
{
    public class PatrimonioController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new PatrimonioViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Controladorias/Patrimonio/Index.cshtml", viewModel);
        }
    }
}