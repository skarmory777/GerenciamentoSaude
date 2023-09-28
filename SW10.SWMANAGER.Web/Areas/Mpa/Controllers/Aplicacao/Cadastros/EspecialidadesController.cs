using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Especialidades;
using SW10.SWMANAGER.Web.Controllers;

using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class EspecialidadesController : SWMANAGERControllerBase
    {
        private readonly IEspecialidadeAppService _especialidadeAppService;

        public EspecialidadesController(
            IEspecialidadeAppService especialidadeAppService
            )
        {
            _especialidadeAppService = especialidadeAppService;
        }


        public ActionResult Index()
        {
            var model = new EspecialidadesViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Especialidades/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Especialidade_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Especialidade_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {

            CriarOuEditarEspecialidadeModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _especialidadeAppService.Obter((long)id);
                viewModel = new CriarOuEditarEspecialidadeModalViewModel(output);
            }
            else
            {

                viewModel = new CriarOuEditarEspecialidadeModalViewModel(new EspecialidadeDto());

            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Especialidades/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> EspecialidadesPorMedico(long id)
        {
            var especialidades = await _especialidadeAppService.ListarPorMedico(id);
            return Json(especialidades, "application/json;charset=UTF-8", JsonRequestBehavior.AllowGet);
        }

    }
}
