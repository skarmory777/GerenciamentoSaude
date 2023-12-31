using Abp;

namespace SW10.SWMANAGER
{
    /// <summary>
    /// This class can be used as a base class for services in this application.
    /// It has some useful objects property-injected and has some basic methods most of services may need to.
    /// It's suitable for non domain nor application service classes.
    /// For domain services inherit <see cref="SWMANAGERDomainServiceBase"/>.
    /// For application services inherit SWMANAGERAppServiceBase.
    /// </summary>
    public abstract class SWMANAGERServiceBase : AbpServiceBase
    {
        protected SWMANAGERServiceBase()
        {
            LocalizationSourceName = SWMANAGERConsts.LocalizationSourceName;
        }
    }
}