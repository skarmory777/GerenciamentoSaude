using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Diagnosticos.ParecerExame;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Diagnosticos
{
    public class ParecerExameController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new ParecerExameViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Diagnosticos/ParecerExame/Index.cshtml", viewModel);
        }
    }
}