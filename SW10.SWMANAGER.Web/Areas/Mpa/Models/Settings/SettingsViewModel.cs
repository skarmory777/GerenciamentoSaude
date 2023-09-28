using Abp.Application.Services.Dto;
using SW10.SWMANAGER.Configuration.Tenants.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Settings
{
    public class SettingsViewModel
    {
        public TenantSettingsEditDto Settings { get; set; }

        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}