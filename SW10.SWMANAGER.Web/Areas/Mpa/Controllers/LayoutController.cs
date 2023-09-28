using Abp.Application.Navigation;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Favoritos;
using SW10.SWMANAGER.Sessions;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Layout;
using SW10.SWMANAGER.Web.Areas.Mpa.Startup;
using SW10.SWMANAGER.Web.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers
{
    [AbpMvcAuthorize]
    public class LayoutController : SWMANAGERControllerBase
    {
        private readonly ISessionAppService _sessionAppService;
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly ILanguageManager _languageManager;
        private readonly IFavoritoAppService _favoritoAppService;

        public LayoutController(
            ISessionAppService sessionAppService,
            IUserNavigationManager userNavigationManager,
            IMultiTenancyConfig multiTenancyConfig,
            ILanguageManager languageManager,
            IFavoritoAppService favoritoAppService
            )
        {
            _sessionAppService = sessionAppService;
            _userNavigationManager = userNavigationManager;
            _multiTenancyConfig = multiTenancyConfig;
            _languageManager = languageManager;
            _favoritoAppService = favoritoAppService;
        }

        [ChildActionOnly]
        public PartialViewResult Header()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = AsyncHelper.RunSync(() => _sessionAppService.GetCurrentLoginInformations()),
                Languages = _languageManager.GetLanguages(),
                CurrentLanguage = _languageManager.CurrentLanguage,
                IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled,
                IsImpersonatedLogin = AbpSession.ImpersonatorUserId.HasValue
            };

            return PartialView("_Header", headerModel);
        }

        [ChildActionOnly]
        public PartialViewResult Sidebar(string currentPageName = "", string menuName = MpaNavigationProvider.MenuName)
        {
            var menus = AsyncHelper.RunSync(() => _userNavigationManager.GetMenusAsync(AbpSession.ToUserIdentifier()));
            var menu = menus.Where(m => m.Name == menuName).FirstOrDefault();
            var sidebarModel = new SidebarViewModel
            {
                Menu = menu, // AsyncHelper.RunSync(() => _userNavigationManager.GetMenusAsync(menuName, AbpSession.ToUserIdentifier())),
                CurrentPageName = currentPageName
            };

            return PartialView("_Sidebar", sidebarModel);
        }

        [ChildActionOnly]
        public PartialViewResult Footer()
        {
            var footerModel = new FooterViewModel
            {
                LoginInformations = AsyncHelper.RunSync(() => _sessionAppService.GetCurrentLoginInformations())
            };

            return PartialView("_Footer", footerModel);
        }

        [ChildActionOnly]
        public PartialViewResult ChatBar()
        {
            return PartialView("_ChatBar");
        }

        [ChildActionOnly]
        public PartialViewResult SidebarButton(string currentPageName = "", string menuName = MpaNavigationProvider.MenuName)
        {
            var menus = AsyncHelper.RunSync(() => _userNavigationManager.GetMenusAsync(AbpSession.ToUserIdentifier()));
            var menu = menus.Where(m => m.Name == menuName).FirstOrDefault();
            var sidebarModel = new SidebarViewModel
            {
                Menu = menu, // AsyncHelper.RunSync(() => _userNavigationManager.GetMenusAsync(menuName, AbpSession.ToUserIdentifier())),
                CurrentPageName = currentPageName
            };

            //return PartialView("_SidebarButton", sidebarModel);
            return PartialView("_SidebarTab", sidebarModel);
        }

        [ChildActionOnly]
        public PartialViewResult SidebarTab(long key, string currentPageName = "", string menuName = MpaNavigationProvider.MenuName)
        {
            var menus = AsyncHelper.RunSync(() => _userNavigationManager.GetMenusAsync(AbpSession.ToUserIdentifier()));
            var menu = menus.FirstOrDefault(m => m.Name == menuName);
            var sidebarModel = new SidebarViewModel
            {
                Menu = menu, // AsyncHelper.RunSync(() => _userNavigationManager.GetMenusAsync(menuName, AbpSession.ToUserIdentifier())),
                CurrentPageName = currentPageName
            };
            ViewBag.Key = key;
            //return PartialView("_SidebarButton", sidebarModel);
            return PartialView("_SidebarTab", sidebarModel);
        }

        [HttpPost]
        public async Task<string> Favoritar(FavoritoDto favorito)
        {
            string url = string.Empty;
            var userId = AbpSession.GetUserId();
            favorito.UserId = userId;
            var favs = await _favoritoAppService.Listar(userId);
            HashSet<string> favoritos = new HashSet<string>();
            foreach (var fav in favs.Items)
            {
                favoritos.Add(fav.Name);
            }

            if (favoritos.Contains(favorito.Name))
            {
                await _favoritoAppService.Excluir(favorito);
                url = "0";
            }
            else
            {
                await _favoritoAppService.CriarOuEditar(favorito);
                url = "1";
            }

            return url;
        }

        public List<FavoritoDto> ListarFavoritos()
        {
            var favoritos = AsyncHelper.RunSync(() => _favoritoAppService.Listar(AbpSession.GetUserId()));
            var favs = favoritos.Items.ToList();

            TempData["Favoritos"] = favs;
            //var f = TempData.Peek("Favoritos") as List<FavoritoDto>;

            return favs;
        }
    }
}