using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Apoios.Manutencao;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Apoios
{
    public class ManutencaoController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new ManutencaoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Apoios/Manutencao/Index.cshtml", viewModel);
        }
    }
}