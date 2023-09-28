using System.Collections.Generic;
using Castle.Core.Internal;

namespace SW10.SWMANAGER.Notifications.Dto
{
    public class UserNotificationsOutputDto
    {
        public UserNotificationsOutputDto(IEnumerable<AppUserNotificationMessageDto> userNotifications)
        {
            UserNotifications = userNotifications;
        }
        
        public UserNotificationsOutputDto(AppUserNotificationMessageDto userNotification)
        {
            UserNotifications = new List<AppUserNotificationMessageDto>  {userNotification};
        }
        
        public bool HasNotification => !UserNotifications.IsNullOrEmpty();

        public IEnumerable<AppUserNotificationMessageDto> UserNotifications { get; set; }
    }
}