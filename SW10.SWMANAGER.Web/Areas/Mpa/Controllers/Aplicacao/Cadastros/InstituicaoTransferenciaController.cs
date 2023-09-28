using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.InstituicoesTransferencia;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.InstituicoesTransferencia.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.InstituicoesTransferencia;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class InstituicoesTransferenciaController : SWMANAGERControllerBase
    {
        private readonly IInstituicaoTransferenciaAppService _InstituicaoTransferenciaAppService;

        public InstituicoesTransferenciaController(
            IInstituicaoTransferenciaAppService InstituicaoTransferenciaAppService
            )
        {
            _InstituicaoTransferenciaAppService = InstituicaoTransferenciaAppService;
        }
        // GET: Mpa/InstituicaoTransferencia
        public ActionResult Index()
        {
            var model = new InstituicoesTransferenciaViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/InstituicoesTransferencia/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_InstituicoesTransferencia_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_InstituicoesTransferencia_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarInstituicaoTransferenciaModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _InstituicaoTransferenciaAppService.Obter((long)id); //_InstituicoesTransferenciaervice.GetInstituicoesTransferencia(new GetInstituicoesTransferenciaInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarInstituicaoTransferenciaModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarInstituicaoTransferenciaModalViewModel(new CriarOuEditarInstituicaoTransferencia());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/InstituicoesTransferencia/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}