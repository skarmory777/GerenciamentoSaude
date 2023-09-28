using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.Authorization.Permissions.Dto;

namespace SW10.SWMANAGER.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
