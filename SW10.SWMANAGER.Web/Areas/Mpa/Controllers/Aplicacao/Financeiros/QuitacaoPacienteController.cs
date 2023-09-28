using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.EntregaContasRecebidas.Dto;
using SW10.SWMANAGER.Web.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Financeiros
{
    public class QuitacaoPacienteController : SWMANAGERControllerBase
    {
        public ActionResult Index()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Financeiros/QuitacaoPaciente/Index.cshtml");
        }

        public ActionResult AbrirConsolidacaoModal(List<EntregaContaInput> input)
        {            
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Financeiros/QuitacaoPaciente/_ConsolidacaoModal.cshtml", input);
        }
    }
}