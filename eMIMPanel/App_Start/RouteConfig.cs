﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace eMIMPanel
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*x}", new { x = @".*\.asmx(/.*)?" });

            routes.MapRoute(
                name: "Click",
                url: "{segment}",
                defaults: new { controller = "Url", action = "Click" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            //routes.MapRoute("routeName", "", new { controller = "Home", action = "Login.aspx" });
        }
    }
}
