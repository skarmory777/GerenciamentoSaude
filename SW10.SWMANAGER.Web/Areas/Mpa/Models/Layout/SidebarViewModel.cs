using System.Linq;
using System.Text;
using Abp.Application.Navigation;
using Abp.Collections.Extensions;
using SW10.SWMANAGER.Web.Areas.Mpa.Startup;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Layout
{
    public class SidebarViewModel
    {
        public UserMenu Menu { get; set; }

        public string CurrentPageName { get; set; }
        public string CurrentPageFather { get; set; }
        
        
        public static StringBuilder GerarMenu(UserMenuItem menuItem, string curentPageName, string applicationPath, bool isFirstRoot = false )
        {
            var isActive = curentPageName == menuItem.Name || !menuItem.Items.IsNullOrEmpty() && menuItem.Items.Any(item => item.Name == curentPageName);
            var strBuilder = new StringBuilder($@"<li class=""{(isFirstRoot? "start": "")} {(isActive ? "active" : "")}"">");
        
            if (menuItem.Items.IsNullOrEmpty())
            {
                strBuilder.Append(GeraMenuItem(menuItem, applicationPath));
            }
            else
            {
                strBuilder.Append(@$"
                    <a href=""javascript:;"" class=""auto"">
                        <i class=""{menuItem.Icon}""></i>
                        <span class=""title"">{menuItem.DisplayName}</span>
                        <span class=""arrow""></span>
                    </a>
                    <ul class=""sub-menu"">");
                foreach (var subMenuItem in menuItem.Items)
                {
                    strBuilder.Append(GerarMenu(subMenuItem, curentPageName, applicationPath));    
                }
                
                strBuilder.Append("</li>");
                strBuilder.Append("</ul>");
            }

            strBuilder.Append("</li>");
            return strBuilder;
        }
        
        private static string calculateMenuUrl(string applicationPath, string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return applicationPath;
            }
            // if (UrlChecker.IsRooted(url))
            // {
            //     return url;
            // }
            return applicationPath + url;
        }

        private static StringBuilder GeraMenuItem(UserMenuItem menuItem, string applicationPath, string target = null, string hiddenValue = null)
        {
            if (menuItem.CustomData != null)
            {
                var cd = (MenuItemCustomData)menuItem.CustomData;
                target = string.Format("{0}={1}", cd.Target.Key, menuItem.Name);   //string.Format("{0}={1}", cd.Target.Key, cd.Target.Value);
                hiddenValue = menuItem.Name;
            }
            return new StringBuilder( 
                $@"<a href=""{calculateMenuUrl(applicationPath,menuItem.Url)}"" {target} data-page-name=""{menuItem.Name}"" onclick=""setActivePage('{menuItem.Name}');"">
                <i class=""{menuItem.Icon}""></i>
                <span class=""title""> {menuItem.DisplayName}</span>
                <input type=""hidden"" value=""{hiddenValue}"" />
                </a>");
        }
    }
}