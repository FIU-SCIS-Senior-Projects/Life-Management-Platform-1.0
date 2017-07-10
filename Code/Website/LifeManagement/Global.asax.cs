using LifeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace LifeManagement
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
       /* protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        // Get Forms Identity From Current User
                        FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;

                        // Create a custom Principal Instance and assign to Current User (with caching)
                        MyPrincipal principal = (MyPrincipal)HttpContext.Current.Cache.Get(id.Name);
                        if (principal == null)
                        {
                            // Create and populate your Principal object with the needed data and Roles.
                            principal = MyBusinessLayerSecurityClass.CreatePrincipal(id, id.Name);
                            HttpContext.Current.Cache.Add(
                            id.Name,
                            principal,
                            null,
                            System.Web.Caching.Cache.NoAbsoluteExpiration,
                            new TimeSpan(0, 30, 0),
                            System.Web.Caching.CacheItemPriority.Default,
                            null);
                        }

                        HttpContext.Current.User = principal;
                    }
                }
            }
        }*/
    }
}
