using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Frequencias;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Frequencias.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.Frequencias;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Assistenciais
{
    public class FrequenciasController : SWMANAGERControllerBase
    {
        private readonly IFrequenciaAppService _frequenciaAppService;

        public FrequenciasController(
            IFrequenciaAppService frequenciaAppService
            )
        {
            _frequenciaAppService = frequenciaAppService;
        }

        public ActionResult Index()
        {
            var model = new FrequenciaViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/Frequencias/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao, AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_Frequencia_Edit)]
        public async Task<PartialViewResult> CriarOuEditar(long? id)
        {
            CriarOuEditarFrequenciaViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _frequenciaAppService.Obter(id.Value);
                viewModel = new CriarOuEditarFrequenciaViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarFrequenciaViewModel(new FrequenciaDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/Frequencias/_CriarOuEditarModal.cshtml", viewModel);
        }


    }

}