using Abp.AutoMapper;
using SW10.SWMANAGER.MultiTenancy;
using SW10.SWMANAGER.MultiTenancy.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Common;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Tenants
{
    [AutoMapFrom(typeof(GetTenantFeaturesForEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesForEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }

        public TenantFeaturesEditViewModel(Tenant tenant, GetTenantFeaturesForEditOutput output)
        {
            Tenant = tenant;
            output.MapTo(this);
        }
    }
}