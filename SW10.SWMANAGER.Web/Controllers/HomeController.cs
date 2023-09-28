using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Controllers
{
    public class HomeController : SWMANAGERControllerBase
    {
        //private readonly ISessionAppService _sessionAppService;
        //public HomeController(
        //    ISessionAppService sessionAppService
        //    )
        //{
        //    _sessionAppService = sessionAppService;
        //}
        public ActionResult Index()
        {
            //if (AbpSession.UserId.HasValue)
            //{
            //    ISessionAppService _sessionAppService = new SessionAppService();
            //    var loginInformations = AsyncHelper.RunSync(() => _sessionAppService.GetCurrentLoginInformations());
            //    if (loginInformations != null)
            //    {
            //        return RedirectToActionPermanent("Index", "Application");
            //    }
            //}
            //return RedirectToActionPermanent("Login", "Account");
            ////return View();
            return RedirectToActionPermanent("Index", "Home", new { area = "Mpa" });
        }
    }
}