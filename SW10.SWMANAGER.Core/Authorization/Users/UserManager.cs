using Abp;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Threading;
using SW10.SWMANAGER.Authorization.Roles;
using SW10.SWMANAGER.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Parametrizacoes;

namespace SW10.SWMANAGER.Authorization.Users
{
    /// <summary>
    /// User manager.
    /// Used to implement domain logic for users.
    /// Extends <see cref="AbpUserManager{TRole,TUser}"/>.
    /// </summary>
    public class UserManager : AbpUserManager<Role, User>
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly RoleManager _roleManager;

        public UserManager(
            UserStore userStore,
            RoleManager roleManager,
            IPermissionManager permissionManager,
            IUnitOfWorkManager unitOfWorkManager,
            ICacheManager cacheManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IOrganizationUnitSettings organizationUnitSettings,
            IdentityEmailMessageService emailService,
            ILocalizationManager localizationManager,
            ISettingManager settingManager,
            IdentitySmsMessageService smsService,
            IUserTokenProviderAccessor userTokenProviderAccessor)
            : base(
                  userStore,
                  roleManager,
                  permissionManager,
                  unitOfWorkManager,
                  cacheManager,
                  organizationUnitRepository,
                  userOrganizationUnitRepository,
                  organizationUnitSettings,
                  localizationManager,
                  emailService,
                  settingManager,
                  userTokenProviderAccessor)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _roleManager = roleManager;

            SmsService = smsService;
        }

        public async Task<User> GetUserOrNullAsync(UserIdentifier userIdentifier)
        {
            using (_unitOfWorkManager.Begin())
            {
                using (_unitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
                {
                    return await FindByIdAsync(userIdentifier.UserId);
                }
            }
        }

        public User GetUserOrNull(UserIdentifier userIdentifier)
        {
            return AsyncHelper.RunSync(() => GetUserOrNullAsync(userIdentifier));
        }

        public async Task<User> GetUserAsync(UserIdentifier userIdentifier)
        {
            var user = await GetUserOrNullAsync(userIdentifier);
            if (user == null)
            {
                throw new ApplicationException("There is no user: " + userIdentifier);
            }

            return user;
        }

        public User GetUser(UserIdentifier userIdentifier)
        {
            return AsyncHelper.RunSync(() => GetUserAsync(userIdentifier));
        }
        
        public async Task<bool> ValidaControleDeIp(string ip, User user)
        {
            //if (IsLocalIpAddress(ip))
            //{
            //    return true;
            //}

            using (var parametrizacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Parametrizacao, long>>())
            using (var parametrizacaoIpsRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ParametrizacaoIp, long>>())
            {
                var parametrizacao = await parametrizacaoRepository.Object.GetAll().AsNoTracking().FirstOrDefaultAsync().ConfigureAwait(false);
                if (!parametrizacao.IsHabilitaControleDeIp)
                {
                    return true;
                }
                
                
                var roleIds = user.Roles.Select(x => x.RoleId);

                if(user.IsHabilitaControleDeIp.HasValue && !user.IsHabilitaControleDeIp.Value)
                {
                    return true;
                }

                if (_roleManager.Roles.Any(x => roleIds.Contains(x.Id) && x.IsHabilitaControleDeIp.HasValue && !x.IsHabilitaControleDeIp.Value))
                {
                    return true;
                }

                return await parametrizacaoIpsRepository.Object.GetAll().AnyAsync(x => x.Descricao == ip).ConfigureAwait(false);
            }
        }
    }
}