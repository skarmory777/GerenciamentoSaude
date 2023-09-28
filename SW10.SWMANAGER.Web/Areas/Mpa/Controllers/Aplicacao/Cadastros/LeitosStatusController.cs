using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.LeitosStatus;
using SW10.SWMANAGER.Web.Controllers;

using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class LeitosStatusController : SWMANAGERControllerBase
    {
        private readonly ILeitoStatusAppService _leitoStatusAppService;

        public LeitosStatusController(
            ILeitoStatusAppService leitoStatusAppService
            )
        {
            _leitoStatusAppService = leitoStatusAppService;
        }

        public ActionResult Index()
        {
            // var tiposAlta = await _leitoStatusTipoAltaAppService.Listar(new ListarLeitosStatusInput());

            var model = new LeitosStatusViewModel();
            //      model.TiposAlta = new SelectList(tiposAlta.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/LeitosStatus/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Atendimento_LeitosStatus_Create, AppPermissions.Pages_Tenant_Cadastros_Atendimento_LeitosStatus_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            //    var tiposAlta = await _leitoStatusTipoAltaAppService.Listar(new ListarLeitosStatusInput());

            CriarOuEditarLeitoStatusModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _leitoStatusAppService.Obter((long)id);
                viewModel = new CriarOuEditarLeitoStatusModalViewModel(output);
                //      viewModel.TiposAlta = new SelectList(tiposAlta.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
            }
            else
            {
                viewModel = new CriarOuEditarLeitoStatusModalViewModel(new CriarOuEditarLeitoStatus());
                //      viewModel.TiposAlta = new SelectList(tiposAlta.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/LeitosStatus/_CriarOuEditarModal.cshtml", viewModel);
        }
    }
}