#region Usings
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SubGrupos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TiposGrupo;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;
#endregion usings

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class FaturamentoGruposController : SWMANAGERControllerBase
    {
        #region Cabecalho

        private readonly IFaturamentoGrupoAppService _grupoAppService;
        private readonly IFaturamentoSubGrupoAppService _subGrupoAppService;
        private readonly IFaturamentoTipoGrupoAppService _tipoGrupoAppService;

        public FaturamentoGruposController(
            IFaturamentoGrupoAppService grupoAppService
            ,
            IFaturamentoSubGrupoAppService subGrupoAppService
            ,
            IFaturamentoTipoGrupoAppService tipoGrupoAppService
            )
        {
            _grupoAppService = grupoAppService;
            _subGrupoAppService = subGrupoAppService;
            _tipoGrupoAppService = tipoGrupoAppService;
        }

        #endregion cabecalho.

        public async Task<ActionResult> Index()
        {
            var model = new FaturamentoGruposViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Grupos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Faturamento_Grupo_Create, AppPermissions.Pages_Tenant_Cadastros_Faturamento_Grupo_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            //var tiposGrupo = await _tipoGrupoAppService.Listar(new ListarFaturamentoTiposGrupoInput());
            //   var subGrupos = await _subGrupoAppService.Listar(new ListarFaturamentoSubGruposInput());

            CriarOuEditarFaturamentoGrupoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _grupoAppService.Obter((long)id);
                viewModel = new CriarOuEditarFaturamentoGrupoModalViewModel(output);
                //    viewModel.SubGrupos = new SelectList(subGrupos.Items, "Id", "Descricao", output.SubGrupoId);
                //    viewModel.TiposGrupo = new SelectList(tiposGrupo.Items, "Id", "Descricao", output.TipoGrupoId);
            }
            else
            {
                viewModel = new CriarOuEditarFaturamentoGrupoModalViewModel(new FaturamentoGrupoDto());
                //    viewModel.SubGrupos = new SelectList(subGrupos.Items, "Id", "Descricao");
                //     viewModel.TiposGrupo = new SelectList(tiposGrupo.Items, "Id", "Descricao");
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Grupos/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}