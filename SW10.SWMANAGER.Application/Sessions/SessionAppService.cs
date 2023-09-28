using Abp.Auditing;
using Abp.Authorization;
using Abp.AutoMapper;
using SW10.SWMANAGER.Sessions.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.Sessions
{
    [AbpAuthorize]
    public class SessionAppService : SWMANAGERAppServiceBase, ISessionAppService
    {
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                User = (await GetCurrentUserAsync()).MapTo<UserLoginInfoDto>()
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = (await GetCurrentTenantAsync()).MapTo<TenantLoginInfoDto>();
            }

            return output;
        }
    }
}