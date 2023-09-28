using Abp.Application.Services.Dto;
using SW10.SWMANAGER.Editions.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.MultiTenancy.Dto
{
    public class GetTenantFeaturesForEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}