using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License.Core.DBContext;
using License.Core.Manager;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(License.MetCalWeb.Startup))]
namespace License.MetCalWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            app.CreatePerOwinContext< AppRoleManager>(AppRoleManager.Create);

            //Intialize AutoMapper
            License.Logic.AutoMapperConfiguration.InitializeAutoMapperConfiguration();

        }
    }
}