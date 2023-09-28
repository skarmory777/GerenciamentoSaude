using SW10.SWMANAGER.Editions;
using SW10.SWMANAGER.EntityFramework;
using System.Linq;

namespace SW10.SWMANAGER.Migrations.Seed.Tenants
{
    public class DefaultTenantBuilder
    {
        private readonly SWMANAGERDbContext _context;

        public DefaultTenantBuilder(SWMANAGERDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateDefaultTenant();
        }

        private void CreateDefaultTenant()
        {
            //Default tenant

            var defaultTenant = _context.Tenants.FirstOrDefault(t => t.TenancyName == MultiTenancy.Tenant.DefaultTenantName);
            if (defaultTenant == null)
            {
                defaultTenant = new MultiTenancy.Tenant(MultiTenancy.Tenant.DefaultTenantName, MultiTenancy.Tenant.DefaultTenantName);

                var defaultEdition = _context.Editions.FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
                if (defaultEdition != null)
                {
                    defaultTenant.EditionId = defaultEdition.Id;
                }

                _context.Tenants.Add(defaultTenant);
                _context.SaveChanges();
            }
        }
    }
}
