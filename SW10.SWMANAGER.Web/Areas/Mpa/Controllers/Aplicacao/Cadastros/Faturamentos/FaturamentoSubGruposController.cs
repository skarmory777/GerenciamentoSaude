#region Usings
using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SubGrupos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SubGrupos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.SubGrupos;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;
#endregion usings.

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class FaturamentoSubGruposController : SWMANAGERControllerBase
    {
        #region Cabecalho

        private readonly IFaturamentoSubGrupoAppService _subGrupoAppService;
        private readonly IFaturamentoGrupoAppService _grupoAppService;


        public FaturamentoSubGruposController(
            IFaturamentoSubGrupoAppService subGrupoAppService
            ,
            IFaturamentoGrupoAppService grupoAppService
            )
        {
            _subGrupoAppService = subGrupoAppService;
            _grupoAppService = grupoAppService;
        }

        #endregion

        public async Task<ActionResult> Index()
        {
            var model = new FaturamentoSubGruposViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/SubGrupos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Faturamento_Grupo_Create, AppPermissions.Pages_Tenant_Cadastros_Faturamento_Grupo_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id, long grupoId = 0)
        {
            CriarOuEditarFaturamentoSubGrupoModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _subGrupoAppService.Obter((long)id);
                viewModel = new CriarOuEditarFaturamentoSubGrupoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarFaturamentoSubGrupoModalViewModel(new FaturamentoSubGrupoDto());
                viewModel.Grupo = new FaturamentoGrupoDto();
                viewModel.Grupo.Descricao = "";
            }

            viewModel.Grupo = AsyncHelper.RunSync(() => _grupoAppService.Obter(grupoId));
            viewModel.GrupoId = grupoId;

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/SubGrupos/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}