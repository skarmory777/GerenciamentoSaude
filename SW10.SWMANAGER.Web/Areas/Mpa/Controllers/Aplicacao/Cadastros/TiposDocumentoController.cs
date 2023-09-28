using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposDocumento;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposDocumento.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposEntrada;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLeito;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLeito.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TiposDocumento;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TiposLeito;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class TiposDocumentoController : SWMANAGERControllerBase
    {
        private readonly ITipoDocumentoAppService _tipoDocumentoAppService;
        private readonly ITipoEntradaAppService _tipoEntradaAppService;

        public TiposDocumentoController(ITipoDocumentoAppService tipoDocumentoAppService
                                      , ITipoEntradaAppService tipoEntradaAppService
            )
        {
            _tipoDocumentoAppService = tipoDocumentoAppService;
            _tipoEntradaAppService = tipoEntradaAppService;
        }
        // GET: Mpa/TipoLeito
        public ActionResult Index()
        {
            var model = new TiposDocumentoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/TiposDocumento/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoDocumento, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoDocumento_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {

            var tiposEntrada = await _tipoEntradaAppService.ListarTodos();

            CriarOuEditarTipoDocumentoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _tipoDocumentoAppService.Obter((long)id); //_TiposLeitoervice.GetTiposLeito(new GetTiposLeitoInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarTipoDocumentoModalViewModel(output);

                viewModel.TiposEntrada = new SelectList(tiposEntrada.Items, "Id", "Descricao", output.TipoEntradaId);
            }
            else
            {
                viewModel = new CriarOuEditarTipoDocumentoModalViewModel(new CriarOuEditarTipoDocumento());
                viewModel.TiposEntrada = new SelectList(tiposEntrada.Items, "Id", "Descricao");
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/TiposDocumento/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}