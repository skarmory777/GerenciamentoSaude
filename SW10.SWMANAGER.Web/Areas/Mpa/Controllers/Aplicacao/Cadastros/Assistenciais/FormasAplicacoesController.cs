using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.FormasAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.FormasAplicacao.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.FormasAplicacoes;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Assistenciais
{
    public class FormasAplicacoesController : SWMANAGERControllerBase
    {
        private readonly IFormaAplicacaoAppService _formaAplicacaoAppService;

        public FormasAplicacoesController(
            IFormaAplicacaoAppService formaAplicacaoAppService
            )
        {
            _formaAplicacaoAppService = formaAplicacaoAppService;
        }

        public ActionResult Index()
        {
            var model = new FormaAplicacaoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/FormasAplicacoes/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao, AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormaAplicacao_Edit)]
        public async Task<PartialViewResult> CriarOuEditar(long? id)
        {
            CriarOuEditarFormaAplicacaoViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _formaAplicacaoAppService.Obter(id.Value);
                viewModel = new CriarOuEditarFormaAplicacaoViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarFormaAplicacaoViewModel(new FormaAplicacaoDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/FormasAplicacoes/_CriarOuEditarModal.cshtml", viewModel);
        }


    }

}