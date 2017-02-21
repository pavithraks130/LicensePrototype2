using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using License.Core.DBContext;
using License.Core.Manager;


[assembly: OwinStartup(typeof(WebApplication1.Startup))]

namespace WebApplication1
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);
        }
    }
}
