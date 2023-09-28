using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Diagnosticos.Laboratorio;
using SW10.SWMANAGER.Web.Controllers;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Diagnosticos
{
    public class LaboratorioController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new LaboratorioViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Diagnosticos/Laboratorio/Index.cshtml", viewModel);
        }
    }
}