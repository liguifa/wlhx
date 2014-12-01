using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace wlhx
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                            name: "See",
                            url: "{controller}/{action}/{id}/{type}/{liid}",
                            defaults: new { controller = "Home", action = "See", id = UrlParameter.Optional, type = UrlParameter.Optional, liid = UrlParameter.Optional }
                        );
            routes.MapRoute(
                            name: "GetDyamicListJson",
                            url: "{controller}/{action}/{id}/{pageIndex}/{pageSize}/{mark}",
                            defaults: new { controller = "User", action = "GetDyamicListJson", id = UrlParameter.Optional, type = UrlParameter.Optional, liid = UrlParameter.Optional, mark = UrlParameter.Optional }
                        );
            routes.MapRoute(
                            name: "Editors",
                            url: "{controller}/{action}/{id}/{dynamicId}",
                            defaults: new { controller = "User", action = "Editors", id = UrlParameter.Optional, dynamicId = UrlParameter.Optional }
                        );

        }
    }
}