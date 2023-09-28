using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.LeitoServicos;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class LeitoServicosController : SWMANAGERControllerBase
    {
        private readonly ILeitoServicoAppService _leitoServicoAppService;

        public LeitoServicosController(
            ILeitoServicoAppService leitoServicoAppService
            )
        {
            _leitoServicoAppService = leitoServicoAppService;
        }

        public ActionResult Index()
        {
            // var tiposAlta = await _leitoServicoTipoAltaAppService.Listar(new ListarLeitoServicosInput());

            var model = new LeitoServicosViewModel();
            //      model.TiposAlta = new SelectList(tiposAlta.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/LeitoServicos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Atendimento_LeitoServicos_Create, AppPermissions.Pages_Tenant_Cadastros_Atendimento_LeitoServicos_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            //    var tiposAlta = await _leitoServicoTipoAltaAppService.Listar(new ListarLeitoServicosInput());

            CriarOuEditarLeitoServicoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _leitoServicoAppService.Obter((long)id);
                viewModel = new CriarOuEditarLeitoServicoModalViewModel(output);
                //      viewModel.TiposAlta = new SelectList(tiposAlta.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
            }
            else
            {
                viewModel = new CriarOuEditarLeitoServicoModalViewModel(new CriarOuEditarLeitoServico());
                //      viewModel.TiposAlta = new SelectList(tiposAlta.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/LeitoServicos/_CriarOuEditarModal.cshtml", viewModel);
        }
    }
}