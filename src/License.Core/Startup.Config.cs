using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Core.DBContext;
using License.Core.Manager;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace License.Core
{
    public class Startup
    {
        public static void ConfigOAuthTokenGeneration(IAppBuilder app)
        {
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
        }
    }
}
