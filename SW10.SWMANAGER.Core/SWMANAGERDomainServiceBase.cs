using System;
using System.Configuration;
using System.Linq;
using Abp.Dependency;
using Abp.Domain.Services;
using Abp.MultiTenancy;
using Abp.Runtime.Session;

namespace SW10.SWMANAGER
{
    public abstract class SWMANAGERDomainServiceBase : DomainService, IDisposable
    {
        /* Add your common members for all your domain services. */
        public IIocResolver IocResolver { get; set; }
        protected SWMANAGERDomainServiceBase()
        {
            LocalizationSourceName = SWMANAGERConsts.LocalizationSourceName;
            AbpSession = IocManager.Instance.ResolveAsDisposable<IAbpSession>();
        }

        public readonly IDisposableDependencyObjectWrapper<IAbpSession> AbpSession;
        public string GetConnection()
        {
            if (!AbpSession.Object.TenantId.HasValue)
            {
                return null;
            }

            using (var tenantCache = IocManager.Instance.ResolveAsDisposable<ITenantCache>())
            {
                var tenant = tenantCache.Object.Get(this.AbpSession.Object.GetTenantId());
                return ConfigurationManager.ConnectionStrings
                    .Cast<ConnectionStringSettings>()
                    .FirstOrDefault(v => string.Compare(v.Name, tenant.TenancyName, StringComparison.OrdinalIgnoreCase) == 0)
                    ?.ConnectionString;
            }
        }

        protected bool Disposed { get; private set; }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }

            if (disposing)
            {
                Disposed = true;
                AbpSession?.Dispose();
                IocResolver?.Release(this);
            }
        }
    }
}
