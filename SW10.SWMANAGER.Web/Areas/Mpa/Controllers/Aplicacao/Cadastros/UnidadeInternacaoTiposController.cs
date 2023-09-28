using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.UnidadesInternacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.UnidadesInternacao;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos.UnidadesInternacao;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class UnidadeInternacaoTiposController : SWMANAGERControllerBase
    {
        private readonly IUnidadeInternacaoTipoAppService _unidadeInternacaoTipoAppService;

        public UnidadeInternacaoTiposController(
            IUnidadeInternacaoTipoAppService unidadeInternacaoTipoAppService
            )
        {
            _unidadeInternacaoTipoAppService = unidadeInternacaoTipoAppService;
        }

        public ActionResult Index()
        {
            //   var tiposAlta = await _unidadeInternacaoTipoAppService.Listar(new ListarUnidadeInternacaoTiposInput());

            var model = new UnidadeInternacaoTiposViewModel();
            // model.Ti = new SelectList(tiposAlta.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/UnidadeInternacaoTipos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Atendimento_UnidadesInternacao_Create, AppPermissions.Pages_Tenant_Cadastros_Atendimento_UnidadesInternacao_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            //   var tiposAlta = await _unidadeInternacaoTipoAppService.Listar(new ListarUnidadeInternacaoTiposInput());

            CriarOuEditarUnidadeInternacaoTipoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _unidadeInternacaoTipoAppService.Obter((long)id);
                viewModel = new CriarOuEditarUnidadeInternacaoTipoModalViewModel(output);
                //     viewModel.TiposAlta = new SelectList(tiposAlta.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
            }
            else
            {
                viewModel = new CriarOuEditarUnidadeInternacaoTipoModalViewModel(new CriarOuEditarUnidadeInternacaoTipo());
                //      viewModel.TiposAlta = new SelectList(tiposAlta.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/UnidadeInternacaoTipos/_CriarOuEditarModal.cshtml", viewModel);
        }
    }
}