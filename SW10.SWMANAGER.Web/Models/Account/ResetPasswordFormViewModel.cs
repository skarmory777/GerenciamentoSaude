﻿using Abp.Auditing;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.Web.Models.Account
{
    public class ResetPasswordFormViewModel
    {
        /// <summary>
        /// Encrypted tenant id.
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// Encrypted user id.
        /// </summary>
        [Required]
        public string UserId { get; set; }

        [Required]
        public string ResetCode { get; set; }

        [Required]
        [DisableAuditing]
        public string Password { get; set; }
    }
}