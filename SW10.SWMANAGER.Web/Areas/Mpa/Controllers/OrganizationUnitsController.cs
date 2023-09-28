using Abp.Domain.Repositories;
using Abp.Organizations;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.Organizations;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.OrganizationUnits;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_OrganizationUnits)]
    public class OrganizationUnitsController : SWMANAGERControllerBase
    {
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IOrganizationUnitAppService _organizationUnitAppService;
        private readonly IUnidadeOrganizacionalAppService _unidadeOrganizacionalAppService;

        public OrganizationUnitsController(
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IOrganizationUnitAppService organizationUnitAppService,
            IUnidadeOrganizacionalAppService unidadeOrganizacionalAppService
            )
        {
            _organizationUnitRepository = organizationUnitRepository;
            _organizationUnitAppService = organizationUnitAppService;
            _unidadeOrganizacionalAppService = unidadeOrganizacionalAppService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public PartialViewResult CreateModal(long? parentId)
        {
            var viewModel = new CriarOuEditarUnidadeOrganizacionalModalViewModel(new UnidadeOrganizacionalDto());
            viewModel.CreateOrganizationUnit = new CreateOrganizationUnitModalViewModel(parentId);
            viewModel.IsAtivo = true;
            return PartialView("~/Areas/Mpa/Views/OrganizationUnits/_CriarOuEditarModal.cshtml", viewModel);

            // Codigo original ABPZero
            //return PartialView("_CreateModal", new CreateOrganizationUnitModalViewModel(parentId));
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<PartialViewResult> EditModal(long id)
        {
            var output = await _unidadeOrganizacionalAppService.Obter(id);
            var viewModel = new CriarOuEditarUnidadeOrganizacionalModalViewModel(output);

            OrganizationUnit organizationUnit = null;

            try
            {
                organizationUnit = _organizationUnitRepository.Get(viewModel.OrganizationUnitId);
            }
            catch { }


            if (organizationUnit == null)
            {
                organizationUnit = new OrganizationUnit();
                organizationUnit.DisplayName = output.Descricao;
                _organizationUnitRepository.Insert(organizationUnit);
            }

            viewModel.CreateOrganizationUnit = new CreateOrganizationUnitModalViewModel(organizationUnit.ParentId);
            viewModel.OrganizationUnitNome = organizationUnit.DisplayName;
            return PartialView("~/Areas/Mpa/Views/OrganizationUnits/_CriarOuEditarModal.cshtml", viewModel);

        }
    }
}