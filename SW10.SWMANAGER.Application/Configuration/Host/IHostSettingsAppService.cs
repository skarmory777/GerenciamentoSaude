using Abp.Application.Services;
using SW10.SWMANAGER.Configuration.Host.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
