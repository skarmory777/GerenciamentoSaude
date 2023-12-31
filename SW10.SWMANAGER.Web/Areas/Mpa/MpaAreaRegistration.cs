﻿using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa
{
    public class MpaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Mpa";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Mpa_default",
                "Mpa/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}