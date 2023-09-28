using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.CentralAutorizacaoGuias;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Faturamentos
{
    public class CentralAutorizacaoController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new CentralAutorizacaoGuiasViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/CentralAutorizacaoGuias/Index.cshtml", viewModel);
        }
    }
}