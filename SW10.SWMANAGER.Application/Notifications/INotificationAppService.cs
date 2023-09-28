using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.Notifications.Dto;
using System;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.Notifications
{
    public interface INotificationAppService : IApplicationService
    {
        Task<GetNotificationsOutput> GetUserNotifications(GetUserNotificationsInput input);

        Task SetAllNotificationsAsRead();

        Task SetNotificationAsRead(EntityDto<Guid> input);

        Task<GetNotificationSettingsOutput> GetNotificationSettings();

        Task UpdateNotificationSettings(UpdateNotificationSettingsInput input);

        UserNotificationsOutputDto GetUserNotification();
        void SetUserNotificationRead(Guid id);
    }
}