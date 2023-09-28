using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Atestados;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Assistenciais
{
    public class AtestadosController : SWMANAGERControllerBase
    {
        private readonly IAtestadoAppService _atestadoAppService;
        private readonly ITipoAtestadoAppService _tipoAtestadoAppService;
        private readonly IModeloAtestadoAppService _modeloAtestadoAppService;

        public AtestadosController(
            IAtestadoAppService atestadoAppService,
            ITipoAtestadoAppService tipoAtestadoAppService,
            IModeloAtestadoAppService modeloAtestadoAppService
            )
        {
            _atestadoAppService = atestadoAppService;
            _tipoAtestadoAppService = tipoAtestadoAppService;
            _modeloAtestadoAppService = modeloAtestadoAppService;
        }
        // GET: Mpa/Atestado
        public ActionResult Index()
        {
            var model = new AtestadoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Atestados/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Assistencial_AtestadoMedico_Create, AppPermissions.Pages_Tenant_Assistencial_AtestadoMedico_Edit)]
        public async Task<PartialViewResult> _CriarOuEditarModal(long? id)
        {
            var tiposAtestados = await _tipoAtestadoAppService.ListarTodos();
            var modelosAtestados = await _modeloAtestadoAppService.ListarTodos();

            CriarOuEditarAtestadoViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _atestadoAppService.Obter((long)id); //_Atestadoservice.GetAtestados(new GetAtestadosInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarAtestadoViewModel(output);
                viewModel.TiposAtestados = new SelectList(tiposAtestados.Items, "Id", "Descricao", output.TipoAtestadoId);
                viewModel.ModelosAtestados = new SelectList(modelosAtestados.Items, "Id", "Titulo", output.ModeloAtestadoId);
            }
            else
            {
                viewModel = new CriarOuEditarAtestadoViewModel(new AtestadoDto());
                viewModel.TiposAtestados = new SelectList(tiposAtestados.Items, "Id", "Descricao");
                viewModel.ModelosAtestados = new SelectList(modelosAtestados.Items, "Id", "Titulo");
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Atestados/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}