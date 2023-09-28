using Abp.Web.Mvc.Authorization;
using Newtonsoft.Json;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Grupos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposClasse;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposClasse.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Grupos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class GruposController : Controller //SWMANAGERControllerBase
    {
        private readonly IGrupoAppService _produtoGrupoAppService;
        private readonly IGrupoClasseAppService _grupoClasseAppService;

        public GruposController(
            IGrupoAppService produtoGrupoAppService,
            IGrupoClasseAppService grupoClasseAppService
            )
        {
            _produtoGrupoAppService = produtoGrupoAppService;
            _grupoClasseAppService = grupoClasseAppService;
        }

        // GET: Mpa/Grupo
        public ActionResult Index()
        {
            var model = new GruposViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Grupos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Grupo_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Grupo_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarGrupoModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _produtoGrupoAppService.Obter((long)id); //_Grupoervice.GetGrupo(new GetGrupoInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                viewModel = new CriarOuEditarGrupoModalViewModel(output);

                var classesList = await _grupoClasseAppService.ListarPorGrupo(id.Value);
                viewModel.ClasseList = JsonConvert.SerializeObject(classesList.Items.ToList());
            }
            else
            {
                viewModel = new CriarOuEditarGrupoModalViewModel(new GrupoDto());
                viewModel.ClasseList = JsonConvert.SerializeObject(new List<GrupoClasseDto>());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Grupos/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _produtoGrupoAppService.ListarAutoComplete(term);
            return Json(query.Items, JsonRequestBehavior.AllowGet);
        }

    }
}