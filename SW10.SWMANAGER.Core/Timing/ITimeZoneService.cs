using Abp.Configuration;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.Timing
{
    public interface ITimeZoneService
    {
        Task<string> GetDefaultTimezoneAsync(SettingScopes scope, int? tenantId);
    }
}
