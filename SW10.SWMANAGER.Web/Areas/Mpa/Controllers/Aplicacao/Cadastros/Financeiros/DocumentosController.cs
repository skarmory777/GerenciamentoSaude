using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Financeiros;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class DocumentosController : SWMANAGERControllerBase
	{
        private readonly IDocumentoAppService _documentoAppService;

        public DocumentosController(IDocumentoAppService documentoAppService)
        {
            _documentoAppService = documentoAppService;
        }



        public ActionResult Index()
		{
			var model = new DocumentoViewModel(new DocumentoDto());
			return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/Documento/Index.cshtml", model);
		}

        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            DocumentoViewModel viewModel = null;
            if (id == null || id == 0)
            {
                viewModel = new DocumentoViewModel(new DocumentoDto());
            }
            else
            {
                var documento = await _documentoAppService.Obter((long)id);

                viewModel = new DocumentoViewModel(documento);
            }


            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/Documento/_CriarOuEditarModal.cshtml", viewModel);
        }


    }
}