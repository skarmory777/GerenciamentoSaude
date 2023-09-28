using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.Authorization.Users.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.Authorization.Users
{
    public interface IUserLoginAppService : IApplicationService
    {
        Task<ListResultDto<UserLoginAttemptDto>> GetRecentUserLoginAttempts();
    }
}
