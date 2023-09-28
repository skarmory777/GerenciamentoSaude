using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNet.Identity;
using SW10.SWMANAGER.Authorization.Permissions;
using SW10.SWMANAGER.Authorization.Permissions.Dto;
using SW10.SWMANAGER.Authorization.Roles;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.Authorization.Users.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Notifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.Authorization.Users
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UserAppService : SWMANAGERAppServiceBase, IUserAppService
    {
        private readonly RoleManager _roleManager;
        private readonly IUserEmailer _userEmailer;
        private readonly IUserListExcelExporter _userListExcelExporter;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IAppNotifier _appNotifier;
        private readonly IRepository<RolePermissionSetting, long> _rolePermissionRepository;
        private readonly IRepository<UserPermissionSetting, long> _userPermissionRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<UserEmpresa, long> _userEmpresas;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UserAppService(
            RoleManager roleManager,
            IUserEmailer userEmailer,
            IUserListExcelExporter userListExcelExporter,
            INotificationSubscriptionManager notificationSubscriptionManager,
            IAppNotifier appNotifier,
            IRepository<RolePermissionSetting, long> rolePermissionRepository,
            IRepository<UserPermissionSetting, long> userPermissionRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<UserEmpresa, long> userEmpresas,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            _roleManager = roleManager;
            _userEmailer = userEmailer;
            _userListExcelExporter = userListExcelExporter;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _appNotifier = appNotifier;
            _rolePermissionRepository = rolePermissionRepository;
            _userPermissionRepository = userPermissionRepository;
            _userRoleRepository = userRoleRepository;
            _userEmpresas = userEmpresas;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<PagedResultDto<UserListDto>> GetUsers(GetUsersInput input)
        {
            var query = UserManager.Users
                .Include(u => u.Roles)
                //.Include(u => u.Medico)
                .WhereIf(input.Role.HasValue, u => u.Roles.Any(r => r.RoleId == input.Role.Value))
                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Name.Contains(input.Filter) ||
                        u.Surname.Contains(input.Filter) ||
                        u.UserName.Contains(input.Filter) ||
                        u.EmailAddress.Contains(input.Filter)
                );

            if (!input.Permission.IsNullOrWhiteSpace())
            {
                query = (from user in query
                         join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                         from ur in urJoined.DefaultIfEmpty()
                         join up in _userPermissionRepository.GetAll() on new { UserId = user.Id, Name = input.Permission } equals new { up.UserId, up.Name } into upJoined
                         from up in upJoined.DefaultIfEmpty()
                         join rp in _rolePermissionRepository.GetAll() on new { RoleId = ur.RoleId, Name = input.Permission } equals new { rp.RoleId, rp.Name } into rpJoined
                         from rp in rpJoined.DefaultIfEmpty()
                         where (up != null && up.IsGranted) || (up == null && rp != null)
                         group user by user into userGrouped
                         select userGrouped.Key);
            }

            var userCount = await query.CountAsync();
            var users = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var userListDtos = users.MapTo<List<UserListDto>>();
            await FillRoleNames(userListDtos);

            return new PagedResultDto<UserListDto>(
                userCount,
                userListDtos
                );
        }

        public async Task<ListResultDto<UserListDto>> GetAllUsers()
        {
            var query = UserManager.Users
                .Include(u => u.Roles)
                .Include(u => u.UserEmpresas);

            query = (from user in query
                     join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                     from ur in urJoined.DefaultIfEmpty()
                     join up in _userPermissionRepository.GetAll() on new { UserId = user.Id } equals new { up.UserId } into upJoined
                     from up in upJoined.DefaultIfEmpty()
                     join rp in _rolePermissionRepository.GetAll() on new { RoleId = ur.RoleId } equals new { rp.RoleId } into rpJoined
                     from rp in rpJoined.DefaultIfEmpty()
                     where (up != null && up.IsGranted) || (up == null && rp != null)
                     group user by user into userGrouped
                     select userGrouped.Key);

            var userCount = await query.CountAsync();
            var users = await query
                .ToListAsync();

            var userListDtos = users.MapTo<List<UserListDto>>();
            await FillRoleNames(userListDtos);

            return new ListResultDto<UserListDto>
            {
                Items = userListDtos
            };
        }

        public async Task<FileDto> GetUsersToExcel()
        {
            var users = await UserManager.Users.Include(u => u.Roles).ToListAsync();
            var userListDtos = users.MapTo<List<UserListDto>>();
            await FillRoleNames(userListDtos);

            return _userListExcelExporter.ExportToFile(userListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Create, AppPermissions.Pages_Administration_Users_Edit)]
        public async Task<GetUserForEditOutput> GetUserForEdit(NullableIdDto<long> input)
        {
            //Getting all available roles
            var userRoleDtos = (await _roleManager.Roles
                .OrderBy(r => r.DisplayName)
                .Select(r => new UserRoleDto
                {
                    RoleId = r.Id,
                    RoleName = r.Name,
                    RoleDisplayName = r.DisplayName
                })
                .ToArrayAsync());

            var output = new GetUserForEditOutput
            {
                Roles = userRoleDtos
            };

            if (!input.Id.HasValue)
            {
                //Creating a new user
                output.User = new UserEditDto { IsActive = true, ShouldChangePasswordOnNextLogin = true };

                foreach (var defaultRole in await _roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
                {
                    var defaultUserRole = userRoleDtos.FirstOrDefault(ur => ur.RoleName == defaultRole.Name);
                    if (defaultUserRole != null)
                    {
                        defaultUserRole.IsAssigned = true;
                    }
                }
            }
            else
            {
                //Editing an existing user
                var user = await UserManager.GetUserByIdAsync(input.Id.Value);
                var empresas = await _userEmpresas.GetAllListAsync(m => m.UserId == input.Id);
                if (empresas.Count() > 0)
                {
                    output.UserEmpresas = empresas.MapTo<List<UserEmpresaDto>>();
                }
                else
                {
                    output.UserEmpresas = new List<UserEmpresaDto>();
                }
                output.User = user.MapTo<UserEditDto>();
                output.ProfilePictureId = user.ProfilePictureId;

                foreach (var userRoleDto in userRoleDtos)
                {
                    userRoleDto.IsAssigned = await UserManager.IsInRoleAsync(input.Id.Value, userRoleDto.RoleName);
                }
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_ChangePermissions)]
        public async Task<GetUserPermissionsForEditOutput> GetUserPermissionsForEdit(EntityDto<long> input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            var permissions = PermissionManager.GetAllPermissions();
            var grantedPermissions = await UserManager.GetGrantedPermissionsAsync(user);

            return new GetUserPermissionsForEditOutput
            {
                Permissions = permissions.MapTo<List<FlatPermissionDto>>().OrderBy(p => p.DisplayName).ToList(),
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList()
            };
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_ChangePermissions)]
        public async Task ResetUserSpecificPermissions(EntityDto<long> input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            await UserManager.ResetAllPermissionsAsync(user);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_ChangePermissions)]
        public async Task UpdateUserPermissions(UpdateUserPermissionsInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            var grantedPermissions = PermissionManager.GetPermissionsFromNamesByValidating(input.GrantedPermissionNames);
            await UserManager.SetGrantedPermissionsAsync(user, grantedPermissions);
        }

        [UnitOfWork]//Atualizado (Pablo 08/08/2017)
        public async Task CreateOrUpdateUser(CreateOrUpdateUserInput input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {

                    if (input.User.Id.HasValue)
                    {
                        await UpdateUserAsync(input);
                    }
                    else
                    {
                        await CreateUserAsync(input);
                    }

                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Delete)]
        public async Task DeleteUser(EntityDto<long> input)
        {
            if (input.Id == AbpSession.GetUserId())
            {
                throw new UserFriendlyException(L("YouCanNotDeleteOwnAccount"));
            }

            var user = await UserManager.GetUserByIdAsync(input.Id);
            CheckErrors(await UserManager.DeleteAsync(user));
        }

        public async Task UnlockUser(EntityDto<long> input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            user.Unlock();
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Edit)]
        protected virtual async Task UpdateUserAsync(CreateOrUpdateUserInput input)
        {
            Debug.Assert(input.User.Id != null, "input.User.Id should be set.");

            var user = await UserManager.FindByIdAsync(input.User.Id.Value);

            //Update user properties
            input.User.MapTo(user); //Passwords is not mapped (see mapping configuration)

            if (input.SetRandomPassword)
            {
                input.User.Password = User.CreateRandomPassword();
            }

            if (!input.User.Password.IsNullOrEmpty())
            {
                CheckErrors(await UserManager.ChangePasswordAsync(user, input.User.Password));
            }

            //Update Epresas
            //var empresas = user.UserEmpresas.ToList();
            var empresas = await _userEmpresas.GetAllListAsync(m => m.UserId == user.Id);
            if (empresas.Count() > 0)
            {
                foreach (var empresa in empresas)
                {
                    await _userEmpresas.DeleteAsync(empresa.Id);
                }
            }
            foreach (var empresa in input.AssignedEmpresas)
            {
                var userEmpresa = new UserEmpresa();
                userEmpresa.EmpresaId = Convert.ToInt64(empresa);
                userEmpresa.UserId = user.Id;
                userEmpresa.IsDeleted = false;
                userEmpresa.IsSistema = false;
                userEmpresa.CreationTime = DateTime.Now;
                userEmpresa.CreatorUserId = AbpSession.UserId;
                userEmpresa.Id = await _userEmpresas.InsertAndGetIdAsync(userEmpresa);
                user.UserEmpresas.Add(userEmpresa);
            }
            var check = await UserManager.UpdateAsync(user);
            CheckErrors(check);

            //Update roles
            var check1 = await UserManager.SetRoles(user, input.AssignedRoleNames);
            CheckErrors(check1);
            //await UserManager.SetRoles(user, input.AssignedRoleNames);

            if (input.SendActivationEmail)
            {
                user.SetNewEmailConfirmationCode();
                await _userEmailer.SendEmailActivationLinkAsync(user, input.User.Password);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Create)]
        protected virtual async Task CreateUserAsync(CreateOrUpdateUserInput input)
        {
            var user = input.User.MapTo<User>(); //Passwords is not mapped (see mapping configuration)
            user.TenantId = AbpSession.TenantId;

            //Set password
            if (!input.User.Password.IsNullOrEmpty())
            {
                CheckErrors(await UserManager.PasswordValidator.ValidateAsync(input.User.Password));
            }
            else
            {
                input.User.Password = User.CreateRandomPassword();
            }

            user.Password = new PasswordHasher().HashPassword(input.User.Password);
            user.ShouldChangePasswordOnNextLogin = input.User.ShouldChangePasswordOnNextLogin;

            //Assign roles
            user.Roles = new Collection<UserRole>();
            foreach (var roleName in input.AssignedRoleNames)
            {
                var role = await _roleManager.GetRoleByNameAsync(roleName);
                user.Roles.Add(new UserRole(AbpSession.TenantId, user.Id, role.Id));
            }
            user.UserEmpresas = new List<UserEmpresa>();
            foreach (var empresa in input.AssignedEmpresas)
            {
                var userEmpresa = new UserEmpresa();
                userEmpresa.EmpresaId = Convert.ToInt64(empresa);
                userEmpresa.UserId = user.Id;
                userEmpresa.IsDeleted = false;
                userEmpresa.IsSistema = false;
                userEmpresa.CreationTime = DateTime.Now;
                userEmpresa.CreatorUserId = AbpSession.UserId;
                user.UserEmpresas.Add(userEmpresa);
            }


            CheckErrors(await UserManager.CreateAsync(user));
            await CurrentUnitOfWork.SaveChangesAsync(); //To get new user's Id.

            //Notifications
            await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());
            await _appNotifier.WelcomeToTheApplicationAsync(user);

            //Send activation email
            if (input.SendActivationEmail)
            {
                user.SetNewEmailConfirmationCode();
                await _userEmailer.SendEmailActivationLinkAsync(user, input.User.Password);
            }
        }

        private async Task FillRoleNames(List<UserListDto> userListDtos)
        {
            /* This method is optimized to fill role names to given list. */

            var distinctRoleIds = (
                from userListDto in userListDtos
                from userListRoleDto in userListDto.Roles
                select userListRoleDto.RoleId
                ).Distinct();

            var roleNames = new Dictionary<int, string>();
            foreach (var roleId in distinctRoleIds)
            {
                roleNames[roleId] = (await _roleManager.GetRoleByIdAsync(roleId)).DisplayName;
            }

            foreach (var userListDto in userListDtos)
            {
                foreach (var userListRoleDto in userListDto.Roles)
                {
                    userListRoleDto.RoleName = roleNames[userListRoleDto.RoleId];
                }

                userListDto.Roles = userListDto.Roles.OrderBy(r => r.RoleName).ToList();
            }
        }

        [AbpAllowAnonymous]
        public async Task<ListResultDto<EmpresaDto>> GetUserEmpresas(long id)
        {
            var query = _userEmpresas
                .GetAll()
                .Where(m => m.UserId == id);

            var result = query
                .Select(m => m.Empresa)
                .ToList()
                .Select(s => EmpresaDto.Mapear(s))
                .ToList();


            //var result = list.Select(m => m.Empresa).ToList();

            return new ListResultDto<EmpresaDto> { Items = result };
        }

        // Listar Dropdown
        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<UserListDto> atendimentosDto = new List<UserListDto>();
            try
            {
                //get com filtro

                var query = from p in UserManager.Users
                    .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Id.ToString().Contains(dropdownInput.search) || m.Name.Contains(dropdownInput.search)
                          )
                            orderby p.UserName ascending
                            select new DropdownItems { id = p.Id, text = p.Name + " " + p.Surname };

                //var query = from p in UserManager.Users
                //          .GetAll()
                //          .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                //              m.Codigo.ToLower().Contains(dropdownInput.search.ToLower()) ||
                //              m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                //          )
                //          .Where(c => c.IsStatus)
                //            orderby p.Ordem ascending
                //            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) };

                //paginação 
                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        // Obter
        public async Task<dynamic> Obter(long id)
        {
            try
            {
                var user = await UserManager.Users
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync().ConfigureAwait(false);

                return new { id = user.Id, name = user.Name }; //user;

                //var user = query.MapTo<UserListDto>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [AbpAllowAnonymous]
        public async Task<User> GetUser()
        {
            var result = await GetCurrentUserAsync();
            return result;
        }
    }
}
