using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Controladorias.Custos;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Controladorias
{
    public class CustosController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new CustosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Controladorias/Custos/Index.cshtml", viewModel);
        }
    }
}