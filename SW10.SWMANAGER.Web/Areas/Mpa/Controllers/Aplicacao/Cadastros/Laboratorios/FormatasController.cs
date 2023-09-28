using Abp.Web.Mvc.Authorization;
using Newtonsoft.Json;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.FormataItems;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.FormataItems.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Formatas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Formatas.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.FormataItems;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.Formatas;
using SW10.SWMANAGER.Web.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Laboratorios
{
    public class FormatasController : SWMANAGERControllerBase
    {
        private readonly IFormataAppService _FormataAppService;
        private readonly IFormataItemAppService _FormataItemAppService;

        public FormatasController(
            IFormataAppService FormataAppService,
            IFormataItemAppService FormataItemAppService
            )
        {
            _FormataAppService = FormataAppService;
            _FormataItemAppService = FormataItemAppService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new FormatasViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Formatas/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Formata_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Formata_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarFormataModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _FormataAppService.Obter((long)id);
                viewModel = new CriarOuEditarFormataModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarFormataModalViewModel(new FormataDto());
                viewModel.FormataItens = JsonConvert.SerializeObject(new List<FormataItemDto>());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Formatas/_CriarOuEditarModal.cshtml", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Formata_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Formata_Edit)]
        public async Task<ActionResult> CriarOuEditarModalItem(long? id, long? formataId)
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

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Formatas/_CriarOuEditarModalItem.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _FormataAppService.ListarAutoComplete(term);
            var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            //  return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}