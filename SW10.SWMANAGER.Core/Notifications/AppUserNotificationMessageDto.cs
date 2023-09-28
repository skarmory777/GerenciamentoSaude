using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Notifications;
using Abp.Timing;
using SW10.SWMANAGER.Chat;

namespace SW10.SWMANAGER.Notifications.Dto
{
    public class AppUserNotificationMessageDto: EntityDto<Guid>, INotificationMessage
    {
        public AppUserNotificationMessageDto()
        {
        }
        
        public AppUserNotificationMessageDto(
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

        public static IEnumerable<AppUserNotificationMessage> Mapear(IEnumerable<AppUserNotificationMessageDto> dtos)
        {
            foreach (var dto in dtos)
            {
                yield return Mapear(dto);
            }
        }
        
        public static IEnumerable<AppUserNotificationMessageDto> Mapear(IEnumerable<AppUserNotificationMessage> entities)
        {
            foreach (var entity in entities)
            {
                yield return Mapear(entity);
            }
        }

        public static AppUserNotificationMessage Mapear(AppUserNotificationMessageDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new AppUserNotificationMessage
            {
                Id = dto.Id,
                Message = dto.Message,
                Source = dto.Source,
                SourceId = dto.SourceId,
                Title = dto.Title,
                CreationTime = dto.CreationTime,
                ReadTime = dto.ReadTime,
                UserBlockConfirmation = dto.UserBlockConfirmation,
                UserId = dto.UserId,
                ReadState = dto.ReadState
            };
        }
        
        public static AppUserNotificationMessageDto Mapear(AppUserNotificationMessage entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new AppUserNotificationMessageDto
            {
                Id = entity.Id,
                Message = entity.Message,
                Source = entity.Source,
                SourceId = entity.SourceId,
                Title = entity.Title,
                CreationTime = entity.CreationTime,
                ReadTime = entity.ReadTime,
                UserBlockConfirmation = entity.UserBlockConfirmation,
                UserId = entity.UserId,
                ReadState = entity.ReadState
            };
        }
    }
}