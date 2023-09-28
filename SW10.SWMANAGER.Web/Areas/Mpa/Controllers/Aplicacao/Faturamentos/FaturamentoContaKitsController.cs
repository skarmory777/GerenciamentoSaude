using Abp.Dependency;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaKits;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.ContasKits;
using SW10.SWMANAGER.Web.Controllers;

using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Faturamentos
{
    public class FaturamentoContaKitsController : SWMANAGERControllerBase
    {
        public ActionResult Index()
        {
            var model = new ContaKitsViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/ContaKits/Index.cshtml", model);
        }

        // [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_ContaKit_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_ContaKit_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id, long? contaId = null)
        {
            CriarOuEditarContaKitModalViewModel viewModel;
            if (id.HasValue)
            {
                using (var _faturamentoContaKitAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaKitAppService>())
                {

                    var output = await _faturamentoContaKitAppService.Object.ObterViewModel((long)id);
                    viewModel = new CriarOuEditarContaKitModalViewModel(output);
                    viewModel.FaturamentoContaId = contaId;

                    viewModel.FaturamentoContaId = contaId;
                }
            }
            else
            {
                viewModel = new CriarOuEditarContaKitModalViewModel(new FaturamentoContaKitDto());
                viewModel.FaturamentoContaId = contaId;
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Faturamentos/ContaKits/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<ActionResult> ContaKit(long? id)
        {
            CriarOuEditarContaKitModalViewModel viewModel;
            if (id.HasValue)
            {
                using (var _faturamentoContaKitAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaKitAppService>())
                {
                    var output = await _faturamentoContaKitAppService.Object.Obter((long)id);
                    viewModel = new CriarOuEditarContaKitModalViewModel(output);
                }
            }
            else
            {
                viewModel = new CriarOuEditarContaKitModalViewModel(new FaturamentoContaKitDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Faturamentos/ContaKits/ContaKit/Index.cshtml", viewModel);
        }

    }
}