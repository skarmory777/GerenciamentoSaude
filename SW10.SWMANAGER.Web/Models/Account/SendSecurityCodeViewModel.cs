using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Models.Account
{
    public class SendSecurityCodeViewModel
    {
        public List<SelectListItem> Providers { get; set; }

        [Required]
        public string SelectedProvider { get; set; }

        public string ReturnUrl { get; set; }

        public bool RememberMe { get; set; }
    }
}