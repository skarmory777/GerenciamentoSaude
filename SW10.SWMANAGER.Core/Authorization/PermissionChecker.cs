using Abp.Authorization;
using SW10.SWMANAGER.Authorization.Roles;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.MultiTenancy;

namespace SW10.SWMANAGER.Authorization
{
    /// <summary>
    /// Implements <see cref="PermissionChecker"/>.
    /// </summary>
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
