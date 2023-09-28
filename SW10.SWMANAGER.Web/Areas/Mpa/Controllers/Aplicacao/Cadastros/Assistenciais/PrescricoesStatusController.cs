using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Assistenciais
{
    public class PrescricoesStatusController : SWMANAGERControllerBase
    {
        private readonly IPrescricaoStatusAppService _prescricaoStatusAppService;

        public PrescricoesStatusController(
            IPrescricaoStatusAppService prescricaoStatusAppService
            )
        {
            _prescricaoStatusAppService = prescricaoStatusAppService;
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoStatus)]
        public ActionResult Index()
        {
            var model = new PrescricoesStatusViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesStatus/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoStatus_Create, AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoStatus_Edit)]
        public async Task<PartialViewResult> CriarOuEditar(long? id)
        {
            CriarOuEditarPrescricaoStatusViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _prescricaoStatusAppService.Obter(id.Value);
                viewModel = new CriarOuEditarPrescricaoStatusViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarPrescricaoStatusViewModel(new PrescricaoStatusDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesStatus/_CriarOuEditarModal.cshtml", viewModel);
        }
    }

}