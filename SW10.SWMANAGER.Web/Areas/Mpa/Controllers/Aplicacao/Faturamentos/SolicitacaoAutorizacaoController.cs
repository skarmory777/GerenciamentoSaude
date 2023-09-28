using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Faturamentos
{
    public class SolicitacaoAutorizacaoController: SWMANAGERControllerBase
    {
        public async Task<ActionResult> Index() {
            return null;
        }

        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            return PartialView("");
        }
    }
}