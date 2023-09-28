using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Financeiros;
using SW10.SWMANAGER.Web.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class GrupoContasAdministrativasController : SWMANAGERControllerBase
    {
        private readonly IGrupoContaAdministrativaAppService _grupoContaAdministrativaAppService;

        public GrupoContasAdministrativasController(IGrupoContaAdministrativaAppService grupoContaAdministrativaAppService)
        {
            _grupoContaAdministrativaAppService = grupoContaAdministrativaAppService;
        }


        public ActionResult Index()
        {
            var model = new GrupoContasAdministrativaViewModel(new GrupoContaAdministrativaDto());
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/GrupoContaAdministrativa/Index.cshtml", model);
        }

        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            GrupoContasAdministrativaViewModel viewModel = null;
            if (id == null || id == 0)
            {
                viewModel = new GrupoContasAdministrativaViewModel(new GrupoContaAdministrativaDto());
                viewModel.SubGrupos = JsonConvert.SerializeObject(new List<SubGrupoContaAdministrativaDto>());
            }
            else
            {
                var grupoContaAdministrativaDto = await _grupoContaAdministrativaAppService.Obter((long)id);

                viewModel = new GrupoContasAdministrativaViewModel(grupoContaAdministrativaDto);

                viewModel.SubGrupos = JsonConvert.SerializeObject(grupoContaAdministrativaDto.SubGruposCntAdm);
            }



            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/GrupoContaAdministrativa/_CriarOuEditarModal.cshtml", viewModel);
        }


    }
}