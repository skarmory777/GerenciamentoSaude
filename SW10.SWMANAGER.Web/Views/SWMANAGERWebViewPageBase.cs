using Abp.Dependency;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Views;

namespace SW10.SWMANAGER.Web.Views
{
    public abstract class SWMANAGERWebViewPageBase : SWMANAGERWebViewPageBase<dynamic>
    {

    }

    public abstract class SWMANAGERWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        public IAbpSession AbpSession { get; private set; }

        protected SWMANAGERWebViewPageBase()
        {
            AbpSession = IocManager.Instance.Resolve<IAbpSession>();
            LocalizationSourceName = SWMANAGERConsts.LocalizationSourceName;
        }
    }
}