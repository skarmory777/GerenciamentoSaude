using Abp.Authorization.Roles;
using SW10.SWMANAGER.Authorization.Users;

namespace SW10.SWMANAGER.Authorization.Roles
{
    /// <summary>
    /// Represents a role in the system.
    /// </summary>
    public class Role : AbpRole<User>
    {
        //Can add application specific role properties here
        public bool? IsHabilitaControleDeIp { get; set; }
        public Role()
        {

        }

        public Role(int? tenantId, string displayName)
            : base(tenantId, displayName)
        {

        }

        public Role(int? tenantId, string name, string displayName)
            : base(tenantId, name, displayName)
        {

        }

        public Role(int? tenantId, string name, string displayName, bool isHabilitaControleDeIp)
            : base(tenantId, name, displayName)
        {
            IsHabilitaControleDeIp = IsHabilitaControleDeIp;
        }
    }
}
