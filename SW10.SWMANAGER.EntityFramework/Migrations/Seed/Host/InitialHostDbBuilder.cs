using SW10.SWMANAGER.EntityFramework;

namespace SW10.SWMANAGER.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly SWMANAGERDbContext _context;

        public InitialHostDbBuilder(SWMANAGERDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
