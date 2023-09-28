using Abp;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.RealTime;
using Abp.Runtime.Session;
using Abp.Timing;
using SW10.SWMANAGER.Chat.Dto;
using SW10.SWMANAGER.Friendships.Cache;
using SW10.SWMANAGER.Friendships.Dto;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Castle.Core.Internal;

namespace SW10.SWMANAGER.Chat
{
    public class ChatAppService : SWMANAGERAppServiceBase, IChatAppService
    {
        [DisableAuditing]
        public GetUserChatFriendsWithSettingsOutput GetUserChatFriendsWithSettings()
        {
            using (var userFriendsCache = IocManager.Instance.ResolveAsDisposable<IUserFriendsCache>())
            using (var onlineClientManager = IocManager.Instance.ResolveAsDisposable<IOnlineClientManager>())
            {
                var cacheItem = userFriendsCache.Object.GetCacheItem(AbpSession.ToUserIdentifier());
                if (cacheItem == null)
                {
                    return null;
                }
                var friends = FriendDto.MapearList(cacheItem.Friends);
                
                if (friends.IsNullOrEmpty())
                {
                    return null;
                }

                foreach (var friend in friends)
                {
                    friend.IsOnline = onlineClientManager.Object.IsOnline(new UserIdentifier(friend.FriendTenantId, friend.FriendUserId));
                }

                return new GetUserChatFriendsWithSettingsOutput
                {
                    Friends = friends.ToList(),
                    ServerTime = Clock.Now
                };
            }
        }

        [DisableAuditing]
        public async Task<ListResultDto<ChatMessageDto>> GetUserChatMessages(GetUserChatMessagesInput input)
        {
            using (var chatMessageRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ChatMessage, long>>())
            {
                var userId = AbpSession.GetUserId();
                var messages = await chatMessageRepository.Object.GetAll()
                    .WhereIf(input.MinMessageId.HasValue, m => m.Id < input.MinMessageId.Value)
                    .Where(m => m.UserId == userId && m.TargetTenantId == input.TenantId &&
                                m.TargetUserId == input.UserId)
                    .OrderByDescending(m => m.CreationTime)
                    .Take(50)
                    .ToListAsync();

                messages.Reverse();

                return new ListResultDto<ChatMessageDto>(ChatMessageDto.MapearList(messages).ToList());
            }
        }

        public async Task MarkAllUnreadMessagesOfUserAsRead(MarkAllUnreadMessagesOfUserAsReadInput input)
        {
            using (var chatMessageRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ChatMessage, long>>())
            using (var userFriendsCache = IocManager.Instance.ResolveAsDisposable<IUserFriendsCache>())
            using (var onlineClientManager = IocManager.Instance.ResolveAsDisposable<IOnlineClientManager>())
            using (var chatCommunicator = IocManager.Instance.ResolveAsDisposable<IChatCommunicator>())
            {
                var userId = AbpSession.GetUserId();
                var messages = await chatMessageRepository.Object
                    .GetAll()
                    .Where(m =>
                        m.UserId == userId &&
                        m.TargetTenantId == input.TenantId &&
                        m.TargetUserId == input.UserId &&
                        m.ReadState == ChatMessageReadState.Unread)
                    .ToListAsync();

                if (!messages.Any())
                {
                    return;
                }

                foreach (var message in messages)
                {
                    message.ChangeReadState(ChatMessageReadState.Read);
                }

                var userIdentifier = AbpSession.ToUserIdentifier();
                var friendIdentifier = input.ToUserIdentifier();

                userFriendsCache.Object.ResetUnreadMessageCount(userIdentifier, friendIdentifier);

                var onlineClients = onlineClientManager.Object.GetAllByUserId(userIdentifier);
                if (onlineClients.Any())
                {
                    chatCommunicator.Object.SendAllUnreadMessagesOfUserReadToClients(onlineClients, friendIdentifier);
                }
            }
        }
    }
}