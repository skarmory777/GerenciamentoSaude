using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.MultiTenancy;

namespace SW10.SWMANAGER.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}
