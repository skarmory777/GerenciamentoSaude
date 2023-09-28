using Castle.Components.DictionaryAdapter;
using SW10.SWMANAGER.Friendships.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Chat.Dto
{
    public class GetUserChatFriendsWithSettingsOutput
    {
        public DateTime ServerTime { get; set; }

        public List<FriendDto> Friends { get; set; }

        public GetUserChatFriendsWithSettingsOutput()
        {
            Friends = new EditableList<FriendDto>();
        }
    }
}