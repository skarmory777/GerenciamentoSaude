using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Planos;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class PlanosController : SWMANAGERControllerBase
    {
        private readonly IPlanoAppService _planoAppService;
        private readonly IConvenioAppService _convenioAppService;

        public PlanosController(
            IPlanoAppService planoAppService,
            IConvenioAppService convenioAppService
            )
        {
            _planoAppService = planoAppService;
            _convenioAppService = convenioAppService;
        }

        public ActionResult Index()
        {
            var model = new PlanosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Planos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Plano_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Plano_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id, long? convenioId = null)
        {
            CriarOuEditarPlanoModalViewModel viewModel;
            var convenios = await _convenioAppService.Listar(new ListarConveniosInput());
            if (id.HasValue)
            {
                var output = await _planoAppService.Obter((long)id);
                viewModel = new CriarOuEditarPlanoModalViewModel(output);
                viewModel.Convenios = new SelectList(convenios.Items, "Id", "NomeFantasia", output.ConvenioId);
            }
            else
            {
                viewModel = new CriarOuEditarPlanoModalViewModel(new CriarOuEditarPlano());
                viewModel.Convenios = new SelectList(convenios.Items, "Id", "NomeFantasia");
            }

            if (convenioId.HasValue)
            {
                viewModel.ConvenioId = (long)convenioId;
                viewModel.Convenio = AsyncHelper.RunSync(() => _convenioAppService.Obter((long)convenioId));
            }


            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Planos/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term, long? convenioId)
        {
            var query = await _planoAppService.ListarAutoComplete(term, convenioId);
            //var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            //return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}