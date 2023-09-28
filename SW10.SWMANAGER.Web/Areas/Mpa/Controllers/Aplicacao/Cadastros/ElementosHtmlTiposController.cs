using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ElementosHtml;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ElementosHtml.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ElementosHtmlTipos;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Globais
{
    public class ElementosHtmlTiposController : SWMANAGERControllerBase
    {
        private readonly IElementoHtmlTipoAppService _elementoHtmlAppService;

        public ElementosHtmlTiposController(
            IElementoHtmlTipoAppService elementoHtmlAppService
            )
        {
            _elementoHtmlAppService = elementoHtmlAppService;
        }
        // GET: Mpa/ElementoHtml
        public ActionResult Index()
        {
            var model = new ElementoHtmlTipoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/ElementosHtmlTipos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_ElementoHtml_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_ElementoHtml_Edit)]
        public async Task<PartialViewResult> _CriarOuEditarModal(long? id)
        {
            CriarOuEditarElementoHtmlTipoViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _elementoHtmlAppService.Obter((long)id); //_ElementosHtmlervice.GetElementosHtml(new GetElementosHtmlInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarElementoHtmlTipoViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarElementoHtmlTipoViewModel(new ElementoHtmlTipoDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/ElementosHtmlTipos/_CriarOuEditarModal.cshtml", viewModel);
        }
    }
}