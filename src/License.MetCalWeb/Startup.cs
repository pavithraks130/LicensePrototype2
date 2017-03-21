using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using System.Web.Mvc;
using System.Web.Routing;

[assembly: OwinStartup(typeof(License.MetCalWeb.Startup))]

namespace License.MetCalWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            License.Logic.AutoMapperConfiguration.InitializeAutoMapperConfiguration();
            LicenseServer.Logic.Initializer.AutoMapperInitializer();

            ConfigureOAuth(app);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                ExpireTimeSpan = TimeSpan.FromMinutes(30)
            });
           
        }
    }
}
