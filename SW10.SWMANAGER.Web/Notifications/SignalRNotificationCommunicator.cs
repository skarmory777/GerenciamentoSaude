using System.Collections.Generic;
using Abp;
using Abp.Dependency;
using Abp.RealTime;
using Castle.Core.Logging;
using Microsoft.AspNet.SignalR;
using SW10.SWMANAGER.Notifications;
using SW10.SWMANAGER.Notifications.Dto;

namespace SW10.SWMANAGER.Web.Notifications
{
    public class SignalRNotificationCommunicator : INotificationCommunicator, ITransientDependency
    {
        /// <summary>
        /// Reference to the logger.
        /// </summary>
        public ILogger Logger { get; set; }

        private IOnlineClientManager _onlineClientManager { get; set; }

        private static IHubContext NotificationHub
        {
            get { return GlobalHost.ConnectionManager.GetHubContext<NotificationHub>(); }
        }

        public SignalRNotificationCommunicator(IOnlineClientManager onlineClientManager)
        {
            _onlineClientManager = onlineClientManager;
            Logger = NullLogger.Instance;
        }

        public void SendUnreadMessages(UserIdentifier user)
        {
            var signalRClient = GetSignalRClientOrNull(user);
            if (signalRClient != null)
            {
                signalRClient.GetUserNotification();
            }
        }

        public void SendMessage(IOnlineClient onlineClient, IEnumerable<AppUserNotificationMessage> messages)
        {
            var signalRClient = GetSignalRClientOrNull(onlineClient);
            if (signalRClient != null)
            {
                signalRClient.SendNotificationToOnline(new UserNotificationsOutputDto(AppUserNotificationMessageDto.Mapear(messages)));
            }
        }

        public void SendUnreadMessages(UserIdentifier user, AppUserNotificationMessage message)
        {
            var signalRClient = GetSignalRClientOrNull(user);
            if (signalRClient != null)
            {
                signalRClient.SendNotificationToOnline(new UserNotificationsOutputDto(AppUserNotificationMessageDto.Mapear(message)));
            }
        }

        public void SendMessage(UserIdentifier user, AppUserNotificationMessage message)
        {
            var signalRClient = GetSignalRClientOrNull(user);
            if (signalRClient != null)
            {
                signalRClient.GetUserNotification(new List<AppUserNotificationMessage> {message});
            }
        }

        public void SendMessages(UserIdentifier user, IEnumerable<AppUserNotificationMessage> messages)
        {
            var signalRClient = GetSignalRClientOrNull(user);
            if (signalRClient != null)
            {
                signalRClient.GetUserNotification(messages);
            }
        }

        public IEnumerable<IOnlineClient> GetOnlineClients()
        {
            return _onlineClientManager.GetAllClients();
        }


        private dynamic GetSignalRClientOrNull(UserIdentifier user)
        {
            if (user == null)
            {
                Logger.Debug("Can not get notification user from SignalR hub!");
                return null;
            }

            if (!_onlineClientManager.IsOnline(user))
            {
                Logger.Debug("User " + user.UserId + " not online");
                return null;
            }

            var signalRClient = NotificationHub.Clients.User(user.UserId.ToString());
            if (signalRClient != null)
            {
                return signalRClient;
            }

            Logger.Debug("Can not get notification user " + user.UserId + " from SignalR hub!");
            return null;

        }

        private dynamic GetSignalRClientOrNull(IOnlineClient client)
        {
            if (client == null)
            {
                Logger.Debug("Can not get notification user from SignalR hub!");
                return null;
            }


            var signalRClient = NotificationHub.Clients.Client(client.ConnectionId);
            if (signalRClient != null)
            {
                return signalRClient;
            }

            Logger.Debug("Can not get notification client connection " + client.UserId + " from SignalR hub!");
            return null;

        }
    }
}