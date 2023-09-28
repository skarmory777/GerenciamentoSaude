using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.HomeCare;
using SW10.SWMANAGER.Web.Controllers;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Atendimentos
{
    public class HomeCareController : SWMANAGERControllerBase
    {
        // GET: Mpa/Exames
        public ActionResult Index()
        {
            var viewModel = new HomeCareViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/HomeCare/Index.cshtml", viewModel);
        }
    }
}