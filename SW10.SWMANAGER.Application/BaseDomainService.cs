using System;
using System.Configuration;
using System.Linq;
using Abp.Dependency;
using Abp.Domain.Services;
using Abp.MultiTenancy;
using Abp.Runtime.Session;

namespace SW10.SWMANAGER
{
    public abstract class BaseDomainService : DomainService, IDisposable
    {
        public BaseDomainService()
        {
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

        public void Dispose()
        {
            AbpSession.Dispose();
        }
    }
}