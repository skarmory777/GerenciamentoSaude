using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Cidades;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class CidadesController : SWMANAGERControllerBase
    {
        private readonly ICidadeAppService _cidadeAppService;
        private readonly IEstadoAppService _estadoAppService;

        public CidadesController(
            ICidadeAppService cidadeAppService,
            IEstadoAppService estadoAppService
            )
        {
            _cidadeAppService = cidadeAppService;
            _estadoAppService = estadoAppService;
        }

        public async Task<ActionResult> Index()
        {
            var estados = await _estadoAppService.Listar(new ListarEstadosInput { MaxResultCount = 50 });

            var model = new CidadesViewModel();
            model.Estados = new SelectList(estados.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1})", m.Nome, m.Uf) }), "Id", "Nome");
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Cidades/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Cidade_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Cidade_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarCidadeModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _cidadeAppService.Obter((long)id);
                viewModel = new CriarOuEditarCidadeModalViewModel(output);
                viewModel.Estado = await _estadoAppService.Obter(output.EstadoId);
            }
            else
            {
                viewModel = new CriarOuEditarCidadeModalViewModel(new CidadeDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Cidades/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term, long? estadoId)
        {
            var query = await _cidadeAppService.ListarAutoComplete(term, estadoId);
            var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            //  return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}