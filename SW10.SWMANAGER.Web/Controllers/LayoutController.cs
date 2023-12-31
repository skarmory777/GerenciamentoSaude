﻿using Abp.Application.Navigation;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.Threading;
using SW10.SWMANAGER.Configuration;
using SW10.SWMANAGER.Sessions;
using SW10.SWMANAGER.Web.Models.Layout;
using SW10.SWMANAGER.Web.Navigation;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Controllers
{
    /// <summary>
    /// Layout for 'front end' pages.
    /// </summary>
    public class LayoutController : SWMANAGERControllerBase
    {
        private readonly ISessionAppService _sessionAppService;
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly ILanguageManager _languageManager;

        public LayoutController(
            ISessionAppService sessionAppService,
            IUserNavigationManager userNavigationManager,
            IMultiTenancyConfig multiTenancyConfig,
            ILanguageManager languageManager)
        {
            _sessionAppService = sessionAppService;
            _userNavigationManager = userNavigationManager;
            _multiTenancyConfig = multiTenancyConfig;
            _languageManager = languageManager;
        }

        [ChildActionOnly]
        public PartialViewResult Header(string currentPageName = "")
        {
            var headerModel = new HeaderViewModel();

            if (AbpSession.UserId.HasValue)
            {
                headerModel.LoginInformations = AsyncHelper.RunSync(() => _sessionAppService.GetCurrentLoginInformations());
            }

            headerModel.Languages = _languageManager.GetLanguages();
            headerModel.CurrentLanguage = _languageManager.CurrentLanguage;

            headerModel.Menu = AsyncHelper.RunSync(() => _userNavigationManager.GetMenuAsync(FrontEndNavigationProvider.MenuName, AbpSession.ToUserIdentifier()));
            headerModel.CurrentPageName = currentPageName;

            headerModel.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;
            headerModel.TenantRegistrationEnabled = SettingManager.GetSettingValue<bool>(AppSettings.TenantManagement.AllowSelfRegistration);

            return PartialView("~/Views/Layout/_Header.cshtml", headerModel);
        }
    }
}