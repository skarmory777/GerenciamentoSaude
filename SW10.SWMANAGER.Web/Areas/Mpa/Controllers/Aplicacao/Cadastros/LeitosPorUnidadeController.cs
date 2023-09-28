using Abp.Domain.Repositories;
using Abp.Organizations;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.Organizations;
using SW10.SWMANAGER.Organizations.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.OrganizationUnits;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_OrganizationUnits)]
    public class LeitosPorUnidadeController : SWMANAGERControllerBase
    {
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;

        private readonly IOrganizationUnitAppService _organizationUnitAppService;
        private readonly ILeitoAppService _leitoAppService;

        public LeitosPorUnidadeController(
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IOrganizationUnitAppService organizationUnitAppService,
            ILeitoAppService leitoAppService
            )
        {
            _organizationUnitRepository = organizationUnitRepository;

            _organizationUnitAppService = organizationUnitAppService;
            _leitoAppService = leitoAppService;
        }

        public ActionResult Index()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/LeitosPorUnidade/Index.cshtml");
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public PartialViewResult CreateModal(long? parentId)
        {
            var viewModel = new CriarOuEditarUnidadeOrganizacionalModalViewModel(new UnidadeOrganizacionalDto());
            viewModel.CreateOrganizationUnit = new CreateOrganizationUnitModalViewModel(parentId);
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/LeitosPorUnidade/_CriarOuEditarModal.cshtml", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<PartialViewResult> EditModal(long id)
        {
            var organizationUnit = await _organizationUnitRepository.GetAsync(id);
            var model = new EditOrganizationUnitModalViewModel { Id = organizationUnit.Id, DisplayName = organizationUnit.DisplayName };

            return PartialView("_EditModal", model);
        }

        public async Task<OrganizationUnitDto> SalvarOrganizationUnit(CreateOrganizationUnitInput ou)
        {
            return await _organizationUnitAppService.CreateOrganizationUnit(ou);
        }

        //public async Task<string> SalvarLeito(CriarOuEditarLeito leito)
        //{
        //    await _leitoAppService.CriarOuEditar(leito);

        //    return "ok";
        //}
    }
}