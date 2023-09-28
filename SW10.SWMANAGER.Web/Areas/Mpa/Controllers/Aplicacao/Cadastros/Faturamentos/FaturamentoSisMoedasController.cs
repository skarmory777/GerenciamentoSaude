using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TiposGrupo;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TiposGrupo.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.SisMoedas;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class FaturamentoSisMoedasController : SWMANAGERControllerBase
    {
        #region Injecao e Contrutor

        private readonly ISisMoedaAppService _sisMoedaAppService;
        private readonly IFaturamentoTipoGrupoAppService _tipoGrupoAppService;


        public FaturamentoSisMoedasController(
            ISisMoedaAppService sisMoedaAppService
                ,
            IFaturamentoTipoGrupoAppService tipoGrupoAppService
            )
        {
            _sisMoedaAppService = sisMoedaAppService;
            _tipoGrupoAppService = tipoGrupoAppService;
        }

        #endregion

        public async Task<ActionResult> Index()
        {
            var model = new SisMoedasViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/SisMoedas/Index.cshtml", model);
        }

        //    [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Faturamento_SisMoeda_Create, AppPermissions.Pages_Tenant_Cadastros_Faturamento_SisMoeda_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            var tiposSisMoeda = await _tipoGrupoAppService.Listar(new ListarFaturamentoTiposGrupoInput());
            //var subSisMoedas = await _subSisMoedaAppService.Listar(new ListarSisMoedasInput());

            CriarOuEditarSisMoedaModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _sisMoedaAppService.Obter((long)id);
                viewModel = new CriarOuEditarSisMoedaModalViewModel(output);
                //viewModel.SubSisMoedas = new SelectList(subSisMoedas.Items, "Id", "Descricao", output.SubSisMoedaId);
                viewModel.Tipos = new SelectList(tiposSisMoeda.Items, "Id", "Descricao", output.Tipo);
            }
            else
            {
                viewModel = new CriarOuEditarSisMoedaModalViewModel(new SisMoedaDto());
                //viewModel.SubSisMoedas = new SelectList(subSisMoedas.Items, "Id", "Descricao");
                viewModel.Tipos = new SelectList(tiposSisMoeda.Items, "Id", "Descricao");
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/SisMoedas/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}