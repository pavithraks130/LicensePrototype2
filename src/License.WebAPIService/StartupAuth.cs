using System;
using System.Threading.Tasks;
using System.Web.Http;
using License.Core.DBContext;
using License.Core.Manager;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(License.WebAPIService.StartupAuth))]

namespace License.WebAPIService
{
    public class StartupAuth
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);
            app.UseWebApi(httpConfig);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            httpConfig.MapHttpAttributeRoutes();
        }
    }
}
