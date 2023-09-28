using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Financeiros.ControleBancario;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Financeiros
{
    public class ControleBancarioController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new ControleBancarioViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Financeiros/ControleBancario/Index.cshtml", viewModel);
        }

        public ActionResult CriarOuEditarModal()
        {
            var viewModel = new ControleBancarioViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Financeiros/ControleBancario/_CriarOuEditarModal.cshtml", viewModel);

        }

        public ActionResult CriarOuEditarModalTransferencia()
        {
            var viewModel = new ControleBancarioViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Financeiros/ControleBancario/_CriarOuEditarModalTransferencia.cshtml", viewModel);
        }
    }
}