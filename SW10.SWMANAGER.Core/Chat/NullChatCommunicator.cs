﻿using Abp;
using Abp.RealTime;
using SW10.SWMANAGER.Friendships;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Chat
{
    public class NullChatCommunicator : IChatCommunicator
    {
        public void SendMessageToClient(IReadOnlyList<IOnlineClient> clients, ChatMessage message)
        {

        }

        public void SendFriendshipRequestToClient(IReadOnlyList<IOnlineClient> clients, Friendship friend, bool isOwnRequest, bool isFriendOnline)
        {

        }

        public void SendUserConnectionChangeToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user, bool isConnected)
        {

        }

        public void SendUserStateChangeToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user, FriendshipState newState)
        {

        }

        public void SendAllUnreadMessagesOfUserReadToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user)
        {

        }
    }
}