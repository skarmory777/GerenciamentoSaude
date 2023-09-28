using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Diagnosticos.Imagens;
using SW10.SWMANAGER.Web.Controllers;

using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Diagnosticos
{
    public class GestaoLaudosController : SWMANAGERControllerBase
    {
        private readonly IRegistroExemesAppService _registroExameService;

        public GestaoLaudosController(IRegistroExemesAppService registroExameService)
        {
            _registroExameService = registroExameService;
        }



        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new LaudoMovimentoViewModel(new LaudoMovimentoDto());
            return View("~/Areas/Mpa/Views/Aplicacao/Diagnosticos/Imagens/GestaoLaudos/Index.cshtml", viewModel);
        }

        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            var viewModel = new GestaoLaudoViewModel(new LaudoMovimentoItemDto());
            if (id != null)
            {
                var laudoMovimentoItem = await _registroExameService.ObterMovimentoItem((long)id);

                if (string.IsNullOrEmpty(laudoMovimentoItem.Laudo))
                {
                    laudoMovimentoItem.Laudo = laudoMovimentoItem.Parecer;
                }

                viewModel = new GestaoLaudoViewModel(laudoMovimentoItem);

                viewModel.IsParecer = true;// laudoMovimentoItem.FaturamentoItem.LaudoGrupo?.Modalidade?.IsParecer;
            }

            return View("~/Areas/Mpa/Views/Aplicacao/Diagnosticos/Imagens/GestaoLaudos/_CriarOuEditarModal.cshtml", viewModel);
        }


        public async Task<ActionResult> EditarLaudoModal(long? id)
        {
            var viewModel = new GestaoLaudoViewModel(new LaudoMovimentoItemDto());
            if (id != null)
            {
                var laudoMovimentoItem = await _registroExameService.ObterMovimentoItem((long)id);

                laudoMovimentoItem.IsEditarLaudo = true;

                viewModel = new GestaoLaudoViewModel(laudoMovimentoItem);

                viewModel.IsParecer = true;// laudoMovimentoItem.FaturamentoItem.LaudoGrupo?.Modalidade?.IsParecer;
            }

            return View("~/Areas/Mpa/Views/Aplicacao/Diagnosticos/Imagens/GestaoLaudos/_CriarOuEditarModal.cshtml", viewModel);
        }




    }
}