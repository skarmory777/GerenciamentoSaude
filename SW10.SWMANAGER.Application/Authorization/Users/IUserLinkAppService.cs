using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.Authorization.Users.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.Authorization.Users
{
    public interface IUserLinkAppService : IApplicationService
    {
        Task LinkToUser(LinkToUserInput linkToUserInput);

        Task<PagedResultDto<LinkedUserDto>> GetLinkedUsers(GetLinkedUsersInput input);

        Task<ListResultDto<LinkedUserDto>> GetRecentlyUsedLinkedUsers();

        Task UnlinkUser(UnlinkUserInput input);
    }
}
