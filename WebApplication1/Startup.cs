using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using License.Core.DBContext;
using License.Core.Manager;
using License.Core.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;


[assembly: OwinStartup(typeof(License.MetCalWeb.Startup))]

namespace License.MetCalWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);

            License.Logic.AutoMapperConfiguration.InitializeAutoMapperConfiguration();

            ConfigureOAuth(app);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions()
            {
                TokenEndpointPath = new PathString("/token"),
                AllowInsecureHttp = true,
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new OAuthAuthorizationServerProvider()
            };

            app.UseOAuthAuthorizationServer(options);
        }
    }
}
