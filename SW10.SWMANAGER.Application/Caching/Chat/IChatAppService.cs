using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.Chat.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.Chat
{
    public interface IChatAppService : IApplicationService
    {
        GetUserChatFriendsWithSettingsOutput GetUserChatFriendsWithSettings();

        Task<ListResultDto<ChatMessageDto>> GetUserChatMessages(GetUserChatMessagesInput input);

        Task MarkAllUnreadMessagesOfUserAsRead(MarkAllUnreadMessagesOfUserAsReadInput input);
    }
}
