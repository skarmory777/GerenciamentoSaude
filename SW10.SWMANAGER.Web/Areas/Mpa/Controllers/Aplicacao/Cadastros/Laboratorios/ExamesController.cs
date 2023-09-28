using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Exames;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.Exames;
using SW10.SWMANAGER.Web.Controllers;

using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Laboratorios
{
    public class ExamesController : SWMANAGERControllerBase
    {
        private readonly IExameAppService _ExameAppService;

        public ExamesController(
            IExameAppService ExameAppService
            )
        {
            _ExameAppService = ExameAppService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new ExamesViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Exames/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Exame_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Exame_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarExameModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _ExameAppService.Obter((long)id);
                viewModel = new CriarOuEditarExameModalViewModel(output);
                if (output.Interpretacao != null)
                {
                    viewModel.InterpretacaoStr = FuncoesGlobais.BlobToStr(output.Interpretacao);
                }
                if (output.Extra1 != null)
                {
                    viewModel.Extra1Str = FuncoesGlobais.BlobToStr(output.Extra1);
                }
                if (output.Extra2 != null)
                {
                    viewModel.Extra2Str = FuncoesGlobais.BlobToStr(output.Extra2);
                }
            }
            else
            {
                viewModel = new CriarOuEditarExameModalViewModel(new FaturamentoItemDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Exames/_CriarOuEditarModal.cshtml", viewModel);
        }



    }
}