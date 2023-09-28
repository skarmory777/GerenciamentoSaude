using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.LeitoCaracteristicas;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class LeitoCaracteristicasController : SWMANAGERControllerBase
    {
        private readonly ILeitoCaracteristicaAppService _leitoCaracteristicaAppService;

        public LeitoCaracteristicasController(
            ILeitoCaracteristicaAppService leitoCaracteristicaAppService
            )
        {
            _leitoCaracteristicaAppService = leitoCaracteristicaAppService;
        }

        public ActionResult Index()
        {
            // var tiposAlta = await _leitoCaracteristicaTipoAltaAppService.Listar(new ListarLeitoCaracteristicasInput());

            var model = new LeitoCaracteristicasViewModel();
            //      model.TiposAlta = new SelectList(tiposAlta.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/LeitoCaracteristicas/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Atendimento_LeitoCaracteristicas_Create, AppPermissions.Pages_Tenant_Cadastros_Atendimento_LeitoCaracteristicas_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            //    var tiposAlta = await _leitoCaracteristicaTipoAltaAppService.Listar(new ListarLeitoCaracteristicasInput());

            CriarOuEditarLeitoCaracteristicaModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _leitoCaracteristicaAppService.Obter((long)id);
                viewModel = new CriarOuEditarLeitoCaracteristicaModalViewModel(output);
                //      viewModel.TiposAlta = new SelectList(tiposAlta.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
            }
            else
            {
                viewModel = new CriarOuEditarLeitoCaracteristicaModalViewModel(new CriarOuEditarLeitoCaracteristica());
                //      viewModel.TiposAlta = new SelectList(tiposAlta.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/LeitoCaracteristicas/_CriarOuEditarModal.cshtml", viewModel);
        }
    }
}