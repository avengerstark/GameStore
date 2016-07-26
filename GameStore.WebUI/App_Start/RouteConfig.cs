using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GameStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            // Важно поместить этот маршрут перед стандартным (Default) маршрутом, который уже присутствует в файле, 
            // т.к. система маршрутизации обрабатывает маршруты в порядке их перечисления
            routes.MapRoute(
                name: null,
                url: "Page{page}",
                defaults: new { controller = "Game", action = "List" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Game", action = "List", id = UrlParameter.Optional }
            );

            
        }
    }
}
