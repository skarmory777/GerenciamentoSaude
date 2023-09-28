using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Interfaces;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Filas;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class FilasController : Controller //SWMANAGERControllerBase
    {
        private readonly IFilaAppService _filaAppService;

        public FilasController(
           IFilaAppService filaAppService
            )
        {
            _filaAppService = filaAppService;
        }

        // GET: Mpa/Grupo
        public ActionResult Index()
        {
            var model = new FilaViewModel(new FilaDto());
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Filas/Index.cshtml", model);
        }

        // [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Grupo_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Grupo_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            FilaViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _filaAppService.Obter(id.Value).ConfigureAwait(false);
                viewModel = new FilaViewModel(output);
            }
            else
            {
                viewModel = new FilaViewModel(new FilaDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Filas/_CriarOuEditarModal.cshtml", viewModel);
        }
    }
}