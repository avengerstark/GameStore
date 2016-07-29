using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GameStore.Domain.Entities;
using GameStore.WebUI.Infrastructure.Binders;

namespace GameStore.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            // Инфраструктуре MVC Framework необходимо сообщить о том, что она может использовать
            // класс CartModelBinder для создания экземпляров Cart.
            ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder());
        }
    }
}
