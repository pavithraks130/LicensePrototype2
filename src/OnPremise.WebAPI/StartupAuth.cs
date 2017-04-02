using System;
using System.Threading.Tasks;
using System.Web.Http;
using License.Core.DBContext;
using License.Core.Manager;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(OnPremise.WebAPI.StartupAuth))]

namespace OnPremise.WebAPI
{
    public class StartupAuth
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();
            //creating the User manager and Role manager instance globally
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            
            //Registering the Web Api Configuration
            WebApiConfig.Register(httpConfig);
            app.UseWebApi(httpConfig);

            //Initializing AutoMapper
            License.Logic.AutoMapperConfiguration.InitializeAutoMapperConfiguration();
        }
    }
}
