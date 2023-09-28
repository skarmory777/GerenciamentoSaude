using Abp.AutoMapper;
using Abp.Modules;
using SW10.SWMANAGER.Authorization;
using System.Reflection;

namespace SW10.SWMANAGER
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(typeof(SWMANAGERCoreModule))]
    public class SWMANAGERApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper mappings
            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                CustomDtoMapper.CreateMappings(mapper);
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
