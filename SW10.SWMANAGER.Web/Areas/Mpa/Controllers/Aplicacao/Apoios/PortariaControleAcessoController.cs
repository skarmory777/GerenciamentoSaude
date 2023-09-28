using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Apoios.PortariaControleAcesso;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Apoios
{
    public class PortariaControleAcessoController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new PortariaControleAcessoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Apoios/PortariaControleAcesso/Index.cshtml", viewModel);
        }
    }
}