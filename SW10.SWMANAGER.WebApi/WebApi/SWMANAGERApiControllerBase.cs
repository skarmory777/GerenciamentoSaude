using Abp.WebApi.Controllers;

namespace SW10.SWMANAGER.WebApi
{
    public abstract class SWMANAGERApiControllerBase : AbpApiController
    {
        protected SWMANAGERApiControllerBase()
        {
            LocalizationSourceName = SWMANAGERConsts.LocalizationSourceName;
        }
    }
}