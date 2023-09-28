using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.Caching.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.Caching
{
    public interface ICachingAppService : IApplicationService
    {
        ListResultDto<CacheDto> GetAllCaches();

        Task ClearCache(EntityDto<string> input);

        Task ClearAllCaches();
    }
}
