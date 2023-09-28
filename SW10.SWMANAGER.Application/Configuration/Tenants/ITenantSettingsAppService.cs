using Abp.Application.Services;
using SW10.SWMANAGER.Configuration.Tenants.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);
    }
}
