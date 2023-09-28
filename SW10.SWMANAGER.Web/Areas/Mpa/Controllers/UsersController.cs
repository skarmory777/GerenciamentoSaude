using Abp.Application.Services.Dto;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.Authorization.Permissions;
using SW10.SWMANAGER.Authorization.Roles;
using SW10.SWMANAGER.Authorization.Roles.Dto;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Users;
using SW10.SWMANAGER.Web.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : SWMANAGERControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly UserManager _userManager;
        private readonly IUserLoginAppService _userLoginAppService;
        private readonly IRoleAppService _roleAppService;
        private readonly IPermissionAppService _permissionAppService;
        private readonly IEmpresaAppService _empresaAppService;
        private readonly IMedicoAppService _medicoAppService;

        public UsersController(
            IUserAppService userAppService,
            UserManager userManager,
            IUserLoginAppService userLoginAppService,
            IRoleAppService roleAppService,
            IPermissionAppService permissionAppService,
            IEmpresaAppService empresaAppService,
            IMedicoAppService medicoAppService
            )
        {
            _userAppService = userAppService;
            _userManager = userManager;
            _userLoginAppService = userLoginAppService;
            _roleAppService = roleAppService;
            _permissionAppService = permissionAppService;
            _empresaAppService = empresaAppService;
            _medicoAppService = medicoAppService;
        }

        public async Task<ActionResult> Index()
        {
            var roles = new List<ComboboxItemDto>();
            var permissions = _permissionAppService.GetAllPermissions()
                                                   .Items
                                                   .Select(p => new ComboboxItemDto(p.Name, new string('-', p.Level * 2) + " " + p.DisplayName))
                                                   .ToList();

            if (IsGranted(AppPermissions.Pages_Administration_Roles))
            {
                var getRolesOutput = await _roleAppService.GetRoles(new GetRolesInput()).ConfigureAwait(false);
                roles = getRolesOutput.Items.Select(r => new ComboboxItemDto(r.Id.ToString(), r.DisplayName)).ToList();
            }

            roles.Insert(0, new ComboboxItemDto("", ""));
            permissions.Insert(0, new ComboboxItemDto("", ""));

            var model = new UsersViewModel
            {
                FilterText = Request.QueryString["filterText"],
                Roles = roles,
                Permissions = permissions
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users_Create, AppPermissions.Pages_Administration_Users_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            var output = await _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id }).ConfigureAwait(false);
            var empresas = await _empresaAppService.ListarTodos().ConfigureAwait(false);
            var medicos = await _medicoAppService.ListarTodos();
            var viewModel = new CreateOrEditUserModalViewModel(output)
            {
                Empresas = empresas.Items.ToList(), Medicos = new SelectList(medicos.Items, "Id", "NomeCompleto")
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users_ChangePermissions)]
        public async Task<PartialViewResult> PermissionsModal(long id)
        {
            var user = await _userManager.GetUserByIdAsync(id).ConfigureAwait(false);
            var output = await _userAppService.GetUserPermissionsForEdit(new EntityDto<long>(id)).ConfigureAwait(false);
            var viewModel = new UserPermissionsEditViewModel(output, user);

            return PartialView("_PermissionsModal", viewModel);
        }

        public async Task<PartialViewResult> LoginAttemptsModal()
        {
            var output = await _userLoginAppService.GetRecentUserLoginAttempts().ConfigureAwait(false);
            var model = new UserLoginAttemptModalViewModel
            {
                LoginAttempts = output.Items.ToList()
            };
            return PartialView("_LoginAttemptsModal", model);
        }
    }
}