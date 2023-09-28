using Abp.UI;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.UnidadesInternacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.UnidadesInternacao;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos.UnidadesInternacao;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class UnidadesInternacaoController : SWMANAGERControllerBase
    {
        private readonly IUnidadeInternacaoAppService _unidadeInternacaoAppService;
        private readonly IUnidadeInternacaoTipoAppService _unidadeInternacaoTipoAppService;

        public UnidadesInternacaoController(
            IUnidadeInternacaoAppService unidadeInternacaoAppService,
            IUnidadeInternacaoTipoAppService unidadeInternacaoTipoAppService
            )
        {
            _unidadeInternacaoAppService = unidadeInternacaoAppService;
            _unidadeInternacaoTipoAppService = unidadeInternacaoTipoAppService;
        }

        public ActionResult Index()
        {
            //   var tiposAlta = await _unidadeInternacaoTipoAppService.Listar(new ListarUnidadesInternacaoInput());

            var model = new UnidadesInternacaoViewModel();
            // model.Ti = new SelectList(tiposAlta.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/UnidadesInternacao/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Atendimento_UnidadesInternacao_Create, AppPermissions.Pages_Tenant_Cadastros_Atendimento_UnidadesInternacao_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            //   var tiposAlta = await _unidadeInternacaoTipoAppService.Listar(new ListarUnidadesInternacaoInput());
            try
            {
                CriarOuEditarUnidadeInternacaoModalViewModel viewModel;
                if (id.HasValue)
                {
                    var output = await _unidadeInternacaoAppService.Obter((long)id);
                    viewModel = new CriarOuEditarUnidadeInternacaoModalViewModel(output);
                    //     viewModel.TiposAlta = new SelectList(tiposAlta.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
                }
                else
                {
                    viewModel = new CriarOuEditarUnidadeInternacaoModalViewModel(new CriarOuEditarUnidadeInternacao());
                    //      viewModel.TiposAlta = new SelectList(tiposAlta.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
                }
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/UnidadesInternacao/_CriarOuEditarModal.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }
        }
    }
}