using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(LifeManagement.Startup))]
namespace LifeManagement
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
        
    }
}