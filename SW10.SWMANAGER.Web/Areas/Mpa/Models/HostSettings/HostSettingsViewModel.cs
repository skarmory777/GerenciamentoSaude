using Abp.Application.Services.Dto;
using SW10.SWMANAGER.Configuration.Host.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.HostSettings
{
    public class HostSettingsViewModel
    {
        public HostSettingsEditDto Settings { get; set; }

        public List<ComboboxItemDto> EditionItems { get; set; }

        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}