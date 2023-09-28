using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Almoxarifado;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Suprimentos
{
    public class AlmoxarifadoController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new AlmoxarifadoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Almoxarifado/Index.cshtml", viewModel);
        }
    }
}