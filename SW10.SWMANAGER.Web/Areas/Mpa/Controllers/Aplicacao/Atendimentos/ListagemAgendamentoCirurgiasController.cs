using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.Relatorios;
using SW10.SWMANAGER.Web.Controllers;

using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Atendimentos
{
    public class ListagemAgendamentoCirurgiasController : SWMANAGERControllerBase
    {
        public ListagemAgendamentoCirurgiasController()
        {

        }

        public async Task<ActionResult> Index()
        {
            var viewModel = new FiltroModel();

            return View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoCirurgias/IndexListagemAgendamento.cshtml", viewModel);
        }



    }
}