using System.Collections.Generic;
using System.Linq;
using Abp;
using Abp.Dependency;
using Abp.RealTime;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using Microsoft.AspNet.SignalR;
using SW10.SWMANAGER.Notifications;
using SW10.SWMANAGER.Notifications.Dto;
using SW10.SWMANAGER.SignalR;

namespace SW10.SWMANAGER.Web.Notifications
{
    public class NotificationHub: BaseHub,ITransientDependency
    {
        /// <summary>
        /// Reference to the logger.
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Reference to the session.
        /// </summary>
        public IAbpSession AbpSession { get; set; }

        private readonly INotificationManager _notificationManager;
        private readonly IOnlineClientManager _onlineClientManager;

        public NotificationHub(INotificationManager notificationManager, IOnlineClientManager onlineClientManager)
        {
            _notificationManager = notificationManager;
            _onlineClientManager = onlineClientManager;

            Logger = NullLogger.Instance;
            AbpSession = NullAbpSession.Instance;
        }
        public UserNotificationsOutputDto GetUserNotification()
        {
            var user = AbpSession.ToUserIdentifier();
            return new UserNotificationsOutputDto(_notificationManager.GetUnreadNotifications(user));
        }
        
        public void SetNotificationRead(AppUserNotificationMessageDto input)
        {
            if (input == null)
            {
                return;
            } 
            var user = AbpSession.ToUserIdentifier();
            _notificationManager.SetReadMessage(user,input.Id);

            foreach (var onlineClient in _onlineClientManager.GetAllClients().Where(x => x.UserId == user.UserId))
            {
                var signalRClient = Clients.Client(onlineClient.ConnectionId);
                if (signalRClient != null)
                {
                    signalRClient.UpdateNotificationToOnline();
                }
            }
        } 
    }
}