using Abp.Dependency;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.PreAtendimentos;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.PreAtendimentos
{
    public class PreAtendimentosController : SWMANAGERControllerBase
    {
        public ActionResult Index()
        {
            TempData["PreAtendimento"] = new PreAtendimentoDto();
            TempData["PreAtendimentoId"] = 0;
            var model = new PreAtendimentosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/PreAtendimentos/Index.cshtml", model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Atendimento_PreAtendimentos_Create, AppPermissions.Pages_Tenant_Atendimento_PreAtendimentos_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                CriarOuEditarAtendimentoModalViewModel viewModel;

                if (id.HasValue)
                {
                    var output = await atendimentoAppService.Object.Obter((long)id).ConfigureAwait(false);
                    viewModel = new CriarOuEditarAtendimentoModalViewModel(output);
                }
                else
                {
                    viewModel = new CriarOuEditarAtendimentoModalViewModel(new AtendimentoDto());
                }

                viewModel.PreAtendimento = true;

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/PreAtendimentos/_CriarOuEditarModal.cshtml", viewModel);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> SalvarPreAtendimento(CriarOuEditarPreAtendimento preAtendimento)
        {
            using (var preAtendimentoAppService = IocManager.Instance.ResolveAsDisposable<IPreAtendimentoAppService>())
            {
                var relacao = new PreAtendimentoDto();
                await preAtendimentoAppService.Object.CriarOuEditar(preAtendimento).ConfigureAwait(false);
                return Content(L("Sucesso"));
            }
        }

        public async Task<PartialViewResult> FormPreAtendimento()
        {
            
            CriarOuEditarAtendimentoModalViewModel viewModel = new CriarOuEditarAtendimentoModalViewModel(new AtendimentoDto());
            //if (1 != 1)
            //{
            //    var output = await _atendimentoAppService.Obter((long)1);
            //    // var output = await _atendimentoAppService.Obter(1);
            //    viewModel = new CriarOuEditarAtendimentoModalViewModel(output);
            //}
            //else
            //{

            //}
            // viewModel.AbaId = abaId;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/IndexParcial.cshtml", viewModel);
        }

    }
}