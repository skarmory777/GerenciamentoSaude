using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Editions.Dto
{
    public class GetEditionForEditOutput
    {
        public EditionEditDto Edition { get; set; }

        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}