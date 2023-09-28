using Abp.AutoMapper;
using SW10.SWMANAGER.Editions.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Common;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Editions
{
    [AutoMapFrom(typeof(GetEditionForEditOutput))]
    public class CreateOrEditEditionModalViewModel : GetEditionForEditOutput, IFeatureEditViewModel
    {
        public bool IsEditMode
        {
            get { return Edition.Id.HasValue; }
        }

        public CreateOrEditEditionModalViewModel(GetEditionForEditOutput output)
        {
            output.MapTo(this);
        }
    }
}