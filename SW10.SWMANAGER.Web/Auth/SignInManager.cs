using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Uow;
using Microsoft.Owin.Security;
using SW10.SWMANAGER.Authorization.Roles;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.MultiTenancy;

namespace SW10.SWMANAGER.Web.Auth
{
    public class SignInManager : AbpSignInManager<Tenant, Role, User>
    {
        public SignInManager(
            UserManager userManager,
            IAuthenticationManager authenticationManager,
            ISettingManager settingManager,
            IUnitOfWorkManager unitOfWorkManager)
            : base(
                  userManager,
                  authenticationManager,
                  settingManager,
                  unitOfWorkManager)
        {
        }
    }
}