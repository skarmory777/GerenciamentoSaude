using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.RecursoGlosa;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Faturamentos
{
    public class RecursoGlosaController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new RecursoGlosaViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/RecursoGlosa/Index.cshtml", viewModel);
        }
    }
}