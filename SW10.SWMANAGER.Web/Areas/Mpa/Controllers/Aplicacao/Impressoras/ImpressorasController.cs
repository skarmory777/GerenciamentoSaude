namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Impressoras
{
    using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Impressoras;
    using SW10.SWMANAGER.Web.Controllers;
    using System.Web.Mvc;

    public class ImpressorasController : SWMANAGERControllerBase
    {
        // GET: Mpa/Impressoras
        public ActionResult ImpressorasModal()
        {
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Impressoras/_ImpressorasModal.cshtml");
        }
        
        public ActionResult ImpressorasLaboratorioModal()
        {
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Impressoras/_ImpressorasLaboratorioModal.cshtml");
        }

        public ActionResult ImprimirMultiplos(string targetAction, string name)
        {
            var viewModel = new ImprimirMultiplosViewModel(targetAction, name);

            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Impressoras/_ImprimirMultiplosModal.cshtml", viewModel);
        }
    }
}