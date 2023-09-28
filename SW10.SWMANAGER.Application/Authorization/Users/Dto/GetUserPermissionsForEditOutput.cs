using SW10.SWMANAGER.Authorization.Permissions.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}