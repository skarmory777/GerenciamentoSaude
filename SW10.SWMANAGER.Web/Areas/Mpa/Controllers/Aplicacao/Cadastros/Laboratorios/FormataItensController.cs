using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.FormataItems;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.FormataItems.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.FormataItems;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Laboratorios
{
    public class FormataItensController : SWMANAGERControllerBase
    {
        private readonly IFormataItemAppService _FormataItemAppService;

        public FormataItensController(
            IFormataItemAppService FormataItemAppService
            )
        {
            _FormataItemAppService = FormataItemAppService;
        }

        public PartialViewResult Index()
        {
            var model = new FormataItemsViewModel();
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/FormataItems/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_FormataItem_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_FormataItem_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id, long formataId)
        {
            CriarOuEditarFormataItemModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _FormataItemAppService.Obter((long)id);
                viewModel = new CriarOuEditarFormataItemModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarFormataItemModalViewModel(new FormataItemDto());
                viewModel.FormataId = formataId;
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/FormataItems/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _FormataItemAppService.ListarAutoComplete(term);
            var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            //  return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}