using EntityFramework.DynamicFilters;
using SW10.SWMANAGER.EntityFramework;

namespace SW10.SWMANAGER.Tests.TestDatas
{
    public class TestDataBuilder
    {
        private readonly SWMANAGERDbContext _context;
        private readonly int _tenantId;

        public TestDataBuilder(SWMANAGERDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new TestOrganizationUnitsBuilder(_context, _tenantId).Create();

            _context.SaveChanges();
        }
    }
}
