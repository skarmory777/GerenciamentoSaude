using Abp;
using Abp.Dependency;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.IO;
using Abp.Modules;
using Abp.Runtime.Caching.Redis;
using Abp.Threading.BackgroundWorkers;
using Abp.Web.Mvc;
using Abp.Zero.Configuration;
using Castle.MicroKernel.Registration;
using Hangfire;
using Microsoft.Owin.Security;
using SW10.SWMANAGER.Background;
using SW10.SWMANAGER.ClassesAplicacao.DisparoDeMensagem;
using SW10.SWMANAGER.ClassesAplicacao.Sefaz;
using SW10.SWMANAGER.Web.Areas.Mpa.Startup;//MPA!
using SW10.SWMANAGER.Web.Background;
using SW10.SWMANAGER.Web.Bundling;
using SW10.SWMANAGER.Web.Navigation;
using SW10.SWMANAGER.Web.Routing;
using SW10.SWMANAGER.WebApi;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Web.SignalR;
using SW10.SWMANAGER.ClassesAplicacao.Avisos;

namespace SW10.SWMANAGER.Web
{
    /// <summary>
    /// Web module of the application.
    /// This is the most top and entrance module that depends on others.
    /// </summary>
    [DependsOn(
        typeof(AbpWebMvcModule),
        typeof(AbpZeroOwinModule),
        typeof(SWMANAGERDataModule),
        typeof(SWMANAGERApplicationModule),
        typeof(SWMANAGERWebApiModule),
        typeof(AbpWebSignalRModule),
        typeof(AbpRedisCacheModule), //AbpRedisCacheModule dependency can be removed if not using Redis cache
        typeof(AbpHangfireModule))] //AbpHangfireModule dependency can be removed if not using Hangfire
    public class SWMANAGERWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<FrontEndNavigationProvider>();
            Configuration.Navigation.Providers.Add<MpaNavigationProvider>();//MPA!

            //Uncomment these lines to use HangFire as background job manager.
            Configuration.BackgroundJobs.UseHangfire(configuration =>
            {
                configuration.GlobalConfiguration.UseSqlServerStorage("Default");
            });

            //Uncomment this line to use Redis cache instead of in-memory cache.
            //Configuration.Caching.UseRedis();
        }

        public override void Initialize()
        {
            //Dependency Injection
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.IocContainer.Register(
                Component
                    .For<IAuthenticationManager>()
                    .UsingFactoryMethod(() => HttpContext.Current.GetOwinContext().Authentication)
                    .LifestyleTransient()
                );

            //Areas
            AreaRegistration.RegisterAllAreas();

            //Routes
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Bundling
            BundleTable.Bundles.IgnoreList.Clear();
            CommonBundleConfig.RegisterBundles(BundleTable.Bundles);
            FrontEndBundleConfig.RegisterBundles(BundleTable.Bundles);
            MpaBundleConfig.RegisterBundles(BundleTable.Bundles);//MPA!
        }

        public override void PostInitialize()
        {
            var server = HttpContext.Current.Server;
            var appFolders = IocManager.Resolve<AppFolders>();
            IocManager.RegisterIfNot<SWMANAGER.Background.IRecurringJobManager, HangfireRecurringJobManager>();

            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            workManager.Add(IocManager.Resolve<DisparoDeMensagemBackgroundWorker>());
            workManager.Add(IocManager.Resolve<AvisoBackgroundWorker>());

            workManager.Add(IocManager.Resolve<SefazTecnospeedSincronizaNotasBackgroundWorker>());

           

            //workManager.s
            //var sefazTecnospeedSincronizaNotasBackgroundWorker = IocManager.Resolve<SefazTecnospeedSincronizaNotasBackgroundWorker>();
            //sefazTecnospeedSincronizaNotasBackgroundWorker.Start();

            appFolders.SampleProfileImagesFolder = server.MapPath("~/Common/Images/SampleProfilePics");
            appFolders.TempFileDownloadFolder = server.MapPath("~/Temp/Downloads");
            appFolders.WebLogsFolder = server.MapPath("~/App_Data/Logs");

            try { DirectoryHelper.CreateIfNotExists(appFolders.TempFileDownloadFolder); } catch { }

            //var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            //workManager.Add(IocManager.Resolve<VisualAsaImportExportWorker>());
            //workManager.Stop();
        }
    }
}
