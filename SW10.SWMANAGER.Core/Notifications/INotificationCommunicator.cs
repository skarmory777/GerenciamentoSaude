using System.Collections.Generic;
using Abp;
using Abp.RealTime;

namespace SW10.SWMANAGER.Notifications
{
    public interface INotificationCommunicator
    {
        IEnumerable<IOnlineClient> GetOnlineClients();
        void SendUnreadMessages(UserIdentifier user, AppUserNotificationMessage message);

        void SendUnreadMessages(UserIdentifier user);
        
        void SendMessage(IOnlineClient onlineClient, IEnumerable<AppUserNotificationMessage> messages);
        
        void SendMessage(UserIdentifier user, AppUserNotificationMessage message);

        void SendMessages(UserIdentifier user, IEnumerable<AppUserNotificationMessage> messages);
    }
}