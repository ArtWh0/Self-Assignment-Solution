using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TechRent.Domain.Concrete;
using TechRent.Domain.Entities;
using TechRent.WebUI.Infrastructure.Binders;

namespace TechRent.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Database.SetInitializer<EFDbContext>(null);
            ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder());
        }
    }
}
