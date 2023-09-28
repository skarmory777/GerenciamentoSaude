using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using Abp.Notifications;
using SW10.SWMANAGER.Notifications.Dto;

namespace SW10.SWMANAGER.Notifications
{
    public interface INotificationManager: IDomainService
    {

        UserNotificationsOutputDto SendUserNotificationToOnline(IUserIdentifier user, IEnumerable<AppUserNotificationMessageDto> message);
        
        void SendNotificationToOnline(IEnumerable<AppUserNotificationMessage> notificationMessages);
        
        void SetReadMessage(IUserIdentifier userIdentifier,AppUserNotificationMessage message);
        
        void SetReadMessage(IUserIdentifier userIdentifier,Guid messageId);

        void SendMessage(UserIdentifier user, AppUserNotificationMessage message, NotificationSeverity severity = NotificationSeverity.Info);
        
        Task SendMessageAsync(UserIdentifier user, AppUserNotificationMessage message, NotificationSeverity severity = NotificationSeverity.Info);
        Task SendMessageAsync(UserIdentifier user, string title, string message, NotificationSeverity severity = NotificationSeverity.Info, string source = null, string sourceId = null);

        Guid Save(IUserIdentifier userIdentifier,AppUserNotificationMessage message);

        IEnumerable<AppUserNotificationMessageDto> GetUnreadNotifications(IUserIdentifier userIdentifier);
        
        IEnumerable<AppUserNotificationMessageDto> GetAllNotificationsBySource(string source, string sourceId);
    }
}