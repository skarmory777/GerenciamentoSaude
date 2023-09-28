using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp;
using Abp.Domain.Entities;
using Abp.Events.Bus;
using Abp.Notifications;
using Abp.Timing;
using SW10.SWMANAGER.Chat;

namespace SW10.SWMANAGER.Notifications
{
    [Table("AppUserNotificationMessages")]
    public class AppUserNotificationMessage: Entity<Guid>, INotificationMessage
    {
        public AppUserNotificationMessage()
        {
        }
        
        public AppUserNotificationMessage(
            IUserIdentifier user,
            string title,
            string message,
            string source,
            string sourceId,
            bool userBlockConfirmation,
            ChatMessageReadState readState)
        {
            UserId = user.UserId;
            Source = source;
            SourceId = sourceId;
            Title = title;
            Message = message;
            ReadState = readState;
            UserBlockConfirmation = userBlockConfirmation;
            CreationTime = Clock.Now;
        }

        public void ChangeReadState(ChatMessageReadState newState)
        {
            ReadState = newState;
            if (newState == ChatMessageReadState.Read)
            {
                ReadTime = Clock.Now;
            }
        }

        public long UserId { get; set; }
        public string Source { get; set; }
        public string SourceId { get; set; }
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }
        
        public ChatMessageReadState ReadState { get; set; }
        
        public bool UserBlockConfirmation { get; set; }
        public DateTime CreationTime { get; set; }
        
        public DateTime? ReadTime { get; set; }
        public UserNotification ToUserNotification()
        {
            var resultItem = new UserNotification();
            return resultItem;
        }
    }
    
    public class NotificationMessageData : NotificationData, INotificationMessage, IEventData
    {
        public NotificationMessageData()
        {
            
        }

        public NotificationMessageData(IUserIdentifier user ,string message, string source, string sourceId)
        {
            UserId = user.UserId;
            Message = message;
            Source = source;
            SourceId = sourceId;
        }
        
        public NotificationMessageData(IUserIdentifier user,string message, string title, string source, string sourceId)
        {
            UserId = user.UserId;
            Message = message;
            Title = title;
            Source = source;
            SourceId = sourceId;
        }
        
        public NotificationMessageData(INotificationMessage message)
        {
            CreationTime = message.CreationTime;
            UserId = message.UserId;
            Title = message.Title;
            Message = message.Message;
            Source = message.Source;
            SourceId = message.SourceId;
            UserBlockConfirmation = message.UserBlockConfirmation;
            ReadState = message.ReadState;
            ReadTime = message.ReadTime;
            EventTime = message.CreationTime;
        }
        public DateTime CreationTime { get; set; }
        public long UserId { get; set; }
        public string Source { get; set; }
        
        public string SourceId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public ChatMessageReadState ReadState { get; set; }
        public bool UserBlockConfirmation { get; set; }
        public DateTime? ReadTime { get; set; }
        public UserNotification ToUserNotification()
        {
            throw new NotImplementedException();
        }

        public DateTime EventTime { get; set; }
        public object EventSource { get; set; }
    }
}