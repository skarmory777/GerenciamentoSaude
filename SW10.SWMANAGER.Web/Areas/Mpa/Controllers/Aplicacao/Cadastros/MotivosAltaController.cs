using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosAlta;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosAlta.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.MotivosAlta;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class MotivosAltaController : SWMANAGERControllerBase
    {
        private readonly IMotivoAltaAppService _motivoAltaAppService;
        private readonly IMotivoAltaTipoAltaAppService _motivoAltaTipoAltaAppService;

        public MotivosAltaController(
            IMotivoAltaAppService motivoAltaAppService,
            IMotivoAltaTipoAltaAppService motivoAltaTipoAltaAppService
            )
        {
            _motivoAltaAppService = motivoAltaAppService;
            _motivoAltaTipoAltaAppService = motivoAltaTipoAltaAppService;
        }

        public async Task<ActionResult> Index()
        {
            var tiposAlta = await _motivoAltaTipoAltaAppService.Listar(new ListarMotivosAltaInput());

            var model = new MotivosAltaViewModel();
            model.TiposAlta = new SelectList(tiposAlta.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/MotivosAlta/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Atendimento_MotivosAlta_Create, AppPermissions.Pages_Tenant_Cadastros_Atendimento_MotivosAlta_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            var tiposAlta = await _motivoAltaTipoAltaAppService.Listar(new ListarMotivosAltaInput());

            CriarOuEditarMotivoAltaModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _motivoAltaAppService.Obter((long)id);
                viewModel = new CriarOuEditarMotivoAltaModalViewModel(output);
                viewModel.TiposAlta = new SelectList(tiposAlta.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
            }
            else
            {
                viewModel = new CriarOuEditarMotivoAltaModalViewModel(new CriarOuEditarMotivoAlta());
                viewModel.TiposAlta = new SelectList(tiposAlta.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/MotivosAlta/_CriarOuEditarModal.cshtml", viewModel);
        }
    }
}