using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using LicenseServer.Core.Manager;
using LicenseServer.Core.DbContext;
using Microsoft.Owin.Security.OAuth;
using Centralized.WebAPI.Common;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(Centralized.WebAPI.Startup))]
namespace Centralized.WebAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();           
            app.CreatePerOwinContext(AppDbContext.Create);
            app.CreatePerOwinContext<LicUserManager>(LicUserManager.Create);
            app.CreatePerOwinContext<LicRoleManager>(LicRoleManager.Create);

            LicenseServer.Logic.Initializer.AutoMapperInitializer();

            ConfigureOAuth(app);
            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);

        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions opt = new OAuthAuthorizationServerOptions()
            {
                TokenEndpointPath = new PathString("/Authenticate"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                AllowInsecureHttp = true,
                Provider = new CustomOAuthProvider()               
            };
            app.UseOAuthBearerTokens(opt);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}