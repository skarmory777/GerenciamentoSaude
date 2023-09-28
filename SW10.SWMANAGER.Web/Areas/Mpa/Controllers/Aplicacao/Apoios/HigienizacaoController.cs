using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Apoios.Higienizacao;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Apoios
{
    public class HigienizacaoController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new HigienizacaoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Apoios/Higienizacao/Index.cshtml", viewModel);
        }
    }
}