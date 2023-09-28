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
    public class PrescricoesItensStatusController : SWMANAGERControllerBase
    {
        private readonly IPrescricaoItemStatusAppService _prescricaoItemStatusAppService;

        public PrescricoesItensStatusController(
            IPrescricaoItemStatusAppService prescricaoItemStatusAppService
            )
        {
            _prescricaoItemStatusAppService = prescricaoItemStatusAppService;
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItemStatus)]
        public ActionResult Index()
        {
            var model = new PrescricoesItensStatusViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItensStatus/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItemStatus_Create, AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItemStatus_Edit)]
        public async Task<PartialViewResult> CriarOuEditar(long? id)
        {
            CriarOuEditarPrescricaoItemStatusViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _prescricaoItemStatusAppService.Obter(id.Value);
                viewModel = new CriarOuEditarPrescricaoItemStatusViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarPrescricaoItemStatusViewModel(new PrescricaoItemStatusDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItensStatus/_CriarOuEditarModal.cshtml", viewModel);
        }
    }

}