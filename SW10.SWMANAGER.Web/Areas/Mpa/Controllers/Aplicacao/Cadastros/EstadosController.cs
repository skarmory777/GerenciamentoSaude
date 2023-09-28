using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Estados;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class EstadosController : SWMANAGERControllerBase
    {
        private readonly IEstadoAppService _estadoAppService;
        private readonly IPaisAppService _paisAppService;

        public EstadosController(
            IEstadoAppService estadoAppService, IPaisAppService paisAppService
            )
        {
            _estadoAppService = estadoAppService;
            _paisAppService = paisAppService;
        }

        public ActionResult Index()
        {
            var model = new EstadosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Estados/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Estado_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Estado_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            var paises = await _paisAppService.Listar(new ListarPaisesInput());

            CriarOuEditarEstadoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _estadoAppService.Obter((long)id);
                viewModel = new CriarOuEditarEstadoModalViewModel(output);
                viewModel.Paises = new SelectList(paises.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1})", m.Nome, m.Sigla) }), "Id", "Nome", output.PaisId);
            }
            else
            {
                viewModel = new CriarOuEditarEstadoModalViewModel(new EstadoDto());
                viewModel.Paises = new SelectList(paises.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1})", m.Nome, m.Sigla) }), "Id", "Nome");
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Estados/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term, long? paisId)
        {
            var query = await _estadoAppService.ListarAutoComplete(term, paisId);
            //var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            //return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}