using SW10.SWMANAGER.Caching.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}