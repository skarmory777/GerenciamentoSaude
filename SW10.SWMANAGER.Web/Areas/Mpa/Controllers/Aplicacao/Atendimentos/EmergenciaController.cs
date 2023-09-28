using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.Emergencia;
using SW10.SWMANAGER.Web.Controllers;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Atendimentos
{
    public class EmergenciaController : SWMANAGERControllerBase
    {
        // GET: Mpa/Emergencia
        public ActionResult Index()
        {
            var viewModel = new EmergenciaViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Emergencia/Index.cshtml", viewModel);
        }
    }
}