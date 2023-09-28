using Abp.Modules;
using Abp.Zero.EntityFramework;
using SW10.SWMANAGER.EntityFramework;
using System.Data.Entity;
using System.Reflection;

namespace SW10.SWMANAGER
{
    /// <summary>
    /// Entity framework module of the application.
    /// </summary>
    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(SWMANAGERCoreModule))]
    public class SWMANAGERDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<SWMANAGERDbContext>());

            //web.config (or app.config for non-web projects) file should contain a connection string named "Default".
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
