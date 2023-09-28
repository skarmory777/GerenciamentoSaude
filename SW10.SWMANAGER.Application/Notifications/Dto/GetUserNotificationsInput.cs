using Abp.Notifications;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }
    }
}