using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ConsultorTabelas;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class ConsultorTabelaCamposController : SWMANAGERControllerBase
    {
        private readonly IConsultorTabelaCampoAppService _campoAppService;
        private readonly IConsultorTabelaAppService _consultorTabelaAppService;
        private readonly IConsultorTipoDadoNFAppService _consultorTipoDadoNFAppService;
        private readonly IConsultorOcorrenciaAppService _consultorOcorrenciaAppService;

        public ConsultorTabelaCamposController(
            IConsultorTabelaCampoAppService campoAppService,
            IConsultorTabelaAppService consultorTabelaAppService,
            IConsultorTipoDadoNFAppService consultorTipoDadoNFAppService,
            IConsultorOcorrenciaAppService consultorOcorrenciaAppService
            )
        {
            _campoAppService = campoAppService;
            _consultorTabelaAppService = consultorTabelaAppService;
            _consultorTipoDadoNFAppService = consultorTipoDadoNFAppService;
            _consultorOcorrenciaAppService = consultorOcorrenciaAppService;
        }

        public ActionResult Index()
        {
            var model = new ConsultorTabelaCamposViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/ConsultorTabelaCampos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Manutencao_Consultor_Tabela_Create, AppPermissions.Pages_Tenant_Manutencao_Consultor_Tabela_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarConsultorTabelaCampoModalViewModel viewModel;
            var tabelas = await _consultorTabelaAppService.Listar(new ListarConsultorTabelasInput());
            var tiposDadoNF = await _consultorTipoDadoNFAppService.ListarTodos();
            var ocorrencias = await _consultorOcorrenciaAppService.ListarTodos();

            if (id.HasValue)
            {
                var output = await _campoAppService.Obter((long)id);
                viewModel = new CriarOuEditarConsultorTabelaCampoModalViewModel(output);
                viewModel.ConsultorTabelas = new SelectList(tabelas.Items, "Id", "Nome");
                viewModel.ConsultorTiposDadoNF = new SelectList(tiposDadoNF.Items, "Id", "Descricao");
                viewModel.ConsultorOcorrencias = new SelectList(ocorrencias.Items, "Id", "Descricao");
            }
            else
            {

                viewModel = new CriarOuEditarConsultorTabelaCampoModalViewModel(new CriarOuEditarConsultorTabelaCampo());
                viewModel.ConsultorTabelas = new SelectList(tabelas.Items, "Id", "Nome");
                viewModel.ConsultorTiposDadoNF = new SelectList(tiposDadoNF.Items, "Id", "Descricao");
                viewModel.ConsultorOcorrencias = new SelectList(ocorrencias.Items, "Id", "Descricao");
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/ConsultorTabelaCampos/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}
