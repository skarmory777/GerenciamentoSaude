using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Naturalidades;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class NaturalidadesController : SWMANAGERControllerBase
    {
        private readonly INaturalidadeAppService _naturalidadeAppService;

        public NaturalidadesController(
            INaturalidadeAppService naturalidadeAppService
            )
        {
            _naturalidadeAppService = naturalidadeAppService;
        }

        public ActionResult Index()
        {
            var model = new NaturalidadesViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Naturalidades/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Naturalidade_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Naturalidade_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarNaturalidadeModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _naturalidadeAppService.Obter((long)id);
                viewModel = new CriarOuEditarNaturalidadeModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarNaturalidadeModalViewModel(new CriarOuEditarNaturalidade());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Naturalidades/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _naturalidadeAppService.ListarAutoComplete(term);
            var result = query.Items.Select(m => new { m.Id, m.Descricao }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}