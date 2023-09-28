﻿using Abp.Notifications;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.Notifications.Dto
{
    public class NotificationSubscriptionDto
    {
        [Required]
        [MaxLength(NotificationInfo.MaxNotificationNameLength)]
        public string Name { get; set; }

        public bool IsSubscribed { get; set; }
    }
}