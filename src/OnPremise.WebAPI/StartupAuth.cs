using System;
using System.Threading.Tasks;
using System.IO;
using System.Web.Http;
using License.Core.DBContext;
using License.Core.Manager;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OAuth;
using OnPremise.WebAPI.Common;
using System.Web.Mvc;

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
            
            //Initializing AutoMapper
            License.Logic.AutoMapperConfiguration.InitializeAutoMapperConfiguration();
            ConfigureLogger();
            ConfigureOAuth(app);
            //Registering the Web Api Configuration
            WebApiConfig.Register(httpConfig);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(httpConfig);

        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions opt = new OAuthAuthorizationServerOptions()
            {
                TokenEndpointPath = new PathString("/Authenticate"),
                AllowInsecureHttp = true,
                Provider = new CustomOAuthProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30)
            };
            app.UseOAuthBearerTokens(opt);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }

        public void ConfigureLogger()
        {
            var folderName = System.Configuration.ConfigurationManager.AppSettings.Get("APILogFolder");
            var appPath = System.Configuration.ConfigurationManager.AppSettings.Get("LogPath");
            var logPath = Path.Combine(appPath, folderName);
            Logger.Logger.ConfigureLogger(logPath,"OnPremiseWebAPI");
        }
    }
}
