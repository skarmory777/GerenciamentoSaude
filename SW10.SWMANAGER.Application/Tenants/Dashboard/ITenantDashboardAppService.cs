using Abp.Application.Services;
using SW10.SWMANAGER.Tenants.Dashboard.Dto;

namespace SW10.SWMANAGER.Tenants.Dashboard
{
    public interface ITenantDashboardAppService : IApplicationService
    {
        GetMemberActivityOutput GetMemberActivity();
    }
}
