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

            // Система маршрутизации обрабатывает маршруты в порядке их перечисления

            // Выводит первую страницу списка товаров всех категорий
            routes.MapRoute(null,
                "",
                new
                {
                    controller = "Game",
                    action = "List",
                    category = (string)null,
                    page = 1
                });
            // Выводит указанную страницу, отображая товары всех категорий
            routes.MapRoute(
                name: null,
                url: "Page{page}",
                defaults: new { controller = "Game", action = "List", category = (string)null },
                constraints: new { page = @"\d+"}
                );

            // Отображает первую страницу элементов указанной категории
            routes.MapRoute(null,
                "{category}",
                new  {controller = "Game", action = "List", page = 1 }
                );
            // Отображает заданную страницу  элементов указанной категории 
            routes.MapRoute(null,
                "{category}/Page{page}",
                new { controller = "Game", action = "List" },
                new  {page = @"\d+" }
                );
            routes.MapRoute(null,"{controller}/{action}");
        }
    }
}
