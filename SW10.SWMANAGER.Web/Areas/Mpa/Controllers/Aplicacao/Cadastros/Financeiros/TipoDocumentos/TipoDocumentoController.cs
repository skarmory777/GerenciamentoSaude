using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Interface;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Financeiros.TipoDocumentos;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Financeiros.TipoDocumentos
{
    public class TipoDocumentoController : SWMANAGERControllerBase
    {

        private readonly ITipoDocumentoAppService _tipoDocumentoAppService;

        public TipoDocumentoController(ITipoDocumentoAppService tipoDocumentoAppService)
        {
            _tipoDocumentoAppService = tipoDocumentoAppService;
        }

        // GET: Mpa/TipoLocalChamada
        public ActionResult Index()
        {
            var model = new TipoDocumentoViewModel(new TipoDocumentoDto());
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/TipoDocumento/Index.cshtml", model);
        }

        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            TipoDocumentoViewModel viewModel = null;
            if (id == null || id == 0)
            {
                viewModel = new TipoDocumentoViewModel(new TipoDocumentoDto());
            }
            else
            {
                var tipoLocalChamadaDto = await _tipoDocumentoAppService.Obter((long)id);

                viewModel = new TipoDocumentoViewModel(tipoLocalChamadaDto);
            }



            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/TipoDocumento/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}