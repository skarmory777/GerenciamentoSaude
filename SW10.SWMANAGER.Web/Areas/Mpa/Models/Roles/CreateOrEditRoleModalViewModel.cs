using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Roles.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Common;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode
        {
            get { return Role.Id.HasValue; }
        }

        public CreateOrEditRoleModalViewModel(GetRoleForEditOutput output)
        {
            output.MapTo(this);
        }
    }
}