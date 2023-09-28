using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Apoios.Nutricao;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Apoios
{
    public class NutricaoController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new NutricaoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Apoios/Nutricao/Index.cshtml", viewModel);
        }
    }
}