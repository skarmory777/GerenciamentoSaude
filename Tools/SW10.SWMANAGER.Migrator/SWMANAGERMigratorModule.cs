using Abp.Events.Bus;
using Abp.Modules;
using Castle.MicroKernel.Registration;
using SW10.SWMANAGER.EntityFramework;
using System.Data.Entity;
using System.Reflection;

namespace SW10.SWMANAGER.Migrator
{
    [DependsOn(typeof(SWMANAGERDataModule))]
    public class SWMANAGERMigratorModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer<SWMANAGERDbContext>(null);
            //            Database.SetInitializer<ReadOnlyContext>(null);

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(typeof(IEventBus), () =>
            {
                IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                );
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}