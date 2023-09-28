using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.Friendships;
using System;

namespace SW10.SWMANAGER.Chat.Dto
{
    [AutoMapFrom(typeof(User))]
    public class ChatUserDto : EntityDto<long>
    {
        public int? TenantId { get; set; }

        public Guid? ProfilePictureId { get; set; }

        public string UserName { get; set; }

        public string TenancyName { get; set; }

        public int UnreadMessageCount { get; set; }

        public bool IsOnline { get; set; }

        public FriendshipState State { get; set; }
    }
}