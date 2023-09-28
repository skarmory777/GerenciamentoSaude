using System;
using Abp.Domain.Entities.Auditing;
using Abp.Notifications;
using SW10.SWMANAGER.Chat;

namespace SW10.SWMANAGER.Notifications
{
    public interface INotificationMessage: IHasCreationTime
    {
        long UserId { get; set; }
        
        string Source { get; set; }
        
        string SourceId { get; set; }
        
        string Title { get; set; }
        string Message { get; set; }
        ChatMessageReadState ReadState { get; set; }
        bool UserBlockConfirmation { get; set; }
        DateTime? ReadTime { get; set; }


        UserNotification ToUserNotification();
    }
}