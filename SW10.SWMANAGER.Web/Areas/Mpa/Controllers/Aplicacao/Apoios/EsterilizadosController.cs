using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Apoios.Esterilizados;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Apoios
{
    public class EsterilizadosController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new EsterilizadosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Apoios/Esterilizados/Index.cshtml", viewModel);
        }
    }
}