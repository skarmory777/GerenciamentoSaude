﻿using Abp;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.RealTime;
using Castle.Core.Logging;
using Microsoft.AspNet.SignalR;
using SW10.SWMANAGER.Chat;
using SW10.SWMANAGER.Chat.Dto;
using SW10.SWMANAGER.Friendships;
using SW10.SWMANAGER.Friendships.Dto;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR.Hubs;

namespace SW10.SWMANAGER.Web.Chat.SignalR
{
    public class SignalRChatCommunicator : IChatCommunicator, ITransientDependency
    {
        /// <summary>
        /// Reference to the logger.
        /// </summary>
        public ILogger Logger { get; set; }

        private static IHubContext ChatHub
        {
            get
            {
                return GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            }
        }

        public SignalRChatCommunicator()
        {
            Logger = NullLogger.Instance;
        }

        public void SendMessageToClient(IReadOnlyList<IOnlineClient> clients, ChatMessage message)
        {
            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    return;
                }

                signalRClient.getChatMessage(ChatMessageDto.Mapear(message));
            }
        }

        public void SendFriendshipRequestToClient(IReadOnlyList<IOnlineClient> clients, Friendship friendship, bool isOwnRequest, bool isFriendOnline)
        {
            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    return;
                }

                var friendshipRequest = FriendDto.Mapear(friendship);
                friendshipRequest.IsOnline = isFriendOnline;

                signalRClient.getFriendshipRequest(friendshipRequest, isOwnRequest);
            }
        }

        public void SendUserConnectionChangeToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user, bool isConnected)
        {
            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    continue;
                }

                signalRClient.getUserConnectNotification(user, isConnected);
            }
        }

        public void SendUserStateChangeToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user, FriendshipState newState)
        {
            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    continue;
                }

                signalRClient.getUserStateChange(user, newState);
            }
        }

        public void SendAllUnreadMessagesOfUserReadToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user)
        {
            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    continue;
                }

                signalRClient.getallUnreadMessagesOfUserRead(user);
            }
        }

        private dynamic GetSignalRClientOrNull(IOnlineClient client)
        {
            var signalRClient = ChatHub.Clients.Client(client.ConnectionId);
            if (signalRClient == null)
            {
                Logger.Debug("Can not get chat user " + client.UserId + " from SignalR hub!");
                return null;
            }

            return signalRClient;
        }
    }
}