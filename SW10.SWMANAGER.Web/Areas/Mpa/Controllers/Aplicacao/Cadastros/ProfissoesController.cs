using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Profissoes;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class ProfissoesController : SWMANAGERControllerBase
    {
        private readonly IProfissaoAppService _profissaoAppService;

        public ProfissoesController(
            IProfissaoAppService profissaoAppService
            )
        {
            _profissaoAppService = profissaoAppService;
        }

        public ActionResult Index()
        {
            var model = new ProfissoesViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Profissoes/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Profissao_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Profissao_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarProfissaoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _profissaoAppService.Obter((long)id);
                viewModel = new CriarOuEditarProfissaoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarProfissaoModalViewModel(new CriarOuEditarProfissao());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Profissoes/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _profissaoAppService.ListarAutoComplete(term);
            return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}