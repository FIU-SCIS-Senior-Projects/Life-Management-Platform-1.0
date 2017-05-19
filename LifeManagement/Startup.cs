using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LifeManagement.Startup))]
namespace LifeManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
