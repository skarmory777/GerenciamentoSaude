using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using Castle.Core.Internal;

namespace SW10.SWMANAGER.Chat.Dto
{
    [AutoMapFrom(typeof(ChatMessage))]
    public class ChatMessageDto : EntityDto
    {
        public long UserId { get; set; }

        public int? TenantId { get; set; }

        public long TargetUserId { get; set; }

        public int? TargetTenantId { get; set; }

        public ChatSide Side { get; set; }

        public ChatMessageReadState ReadState { get; set; }

        public string Message { get; set; }

        public DateTime CreationTime { get; set; }


        public static IEnumerable<ChatMessage> MapearList(IEnumerable<ChatMessageDto> dtos)
        {
            foreach (var dto in dtos)
            {
                yield return Mapear(dto);
            }
        }
        
        public static IEnumerable<ChatMessageDto> MapearList(IEnumerable<ChatMessage> entities)
        {
            foreach (var entity in entities)
            {
                yield return Mapear(entity);
            }
        }
        
        
        public static ChatMessageDto Mapear(ChatMessage entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new ChatMessageDto
            {
                Id = (int) entity.Id,
                TenantId = entity.TenantId,
                UserId = entity.UserId,
                TargetTenantId = entity.TargetTenantId,
                TargetUserId = entity.TargetUserId,
                Message = entity.Message,
                CreationTime = entity.CreationTime,
                Side = entity.Side,
                ReadState = entity.ReadState,
            };

        }
        
        public static ChatMessage Mapear(ChatMessageDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var entity = new ChatMessage
            {
                Id = dto.Id,
                TenantId = dto.TenantId,
                UserId = dto.UserId,
                TargetTenantId = dto.TargetTenantId,
                TargetUserId = dto.TargetUserId,
                Message = dto.Message,
                CreationTime = dto.CreationTime,
                Side = dto.Side
            };

            entity.ChangeReadState(dto.ReadState);
            return entity;

        }

    }
}