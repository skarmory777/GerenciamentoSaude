using Abp.Castle.Logging.Log4Net;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Localization;
using Abp.Logging;
using Abp.Timing;
using Abp.Web;
using Castle.Facilities.Logging;
using Microsoft.ApplicationInsights.Extensibility;
using SW10.SWMANAGER.Web.MultiTenancy;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Web;

namespace SW10.SWMANAGER.Web
{
    public class MvcApplication : AbpWebApplication<SWMANAGERWebModule>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            DisableApplicationInsightsOnDebug();
            //Use UTC clock. Remove this to use local time for your application.
            //Clock.Provider = ClockProviders.Utc;

            //Log4Net configuration
            AbpBootstrapper.IocManager.IocContainer
                .AddFacility<LoggingFacility>(f => f.UseAbpLog4Net()
                    .WithConfig("log4net.config")
                );

            base.Application_Start(sender, e);

            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }

        protected override void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            base.Application_AuthenticateRequest(sender, e);

            SetTentantId();
        }

        protected override void Session_Start(object sender, EventArgs e)
        {
            RestoreUserLanguage();
            base.Session_Start(sender, e);
        }

        private void RestoreUserLanguage()
        {
            using (var settingManager = AbpBootstrapper.IocManager.ResolveAsDisposable<ISettingManager>())
            {
                var defaultLanguage = settingManager.Object.GetSettingValue(LocalizationSettingNames.DefaultLanguage);

                if (defaultLanguage.IsNullOrEmpty())
                {
                    return;
                }

                try
                {
                    CultureInfo.GetCultureInfo(defaultLanguage);
                    Response.Cookies.Add(new HttpCookie("Abp.Localization.CultureName", defaultLanguage) { Expires = Clock.Now.AddYears(2) });
                }
                catch (CultureNotFoundException exception)
                {
                    LogHelper.Logger.Warn(exception.Message, exception);
                }
            }
        }

        /// <summary>
        /// This method tries to set current tenant id if current user has not login.
        /// Thus, we can get IAbpSession.TenantId later.
        /// </summary>
        private void SetTentantId()
        {
            if (User?.Identity != null && User.Identity.IsAuthenticated)
            {
                return;
            }

            using (var currentTenantAccessor = AbpBootstrapper.IocManager.ResolveAsDisposable<TenantIdAccessor>())
            {
                currentTenantAccessor.Object.SetCurrentTenantId();
            }
        }

        /// <summary>
        /// Disables the application insights locally.
        /// </summary>
        [Conditional("DEBUG")]
        private static void DisableApplicationInsightsOnDebug()
        {
            TelemetryConfiguration.Active.DisableTelemetry = true;
        }
    }
}
