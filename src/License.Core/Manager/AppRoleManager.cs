using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Core.DBContext;
using License.Core.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace License.Core.Manager
{
    public class AppRoleManager : RoleManager<Role>
    {
        public AppRoleManager(IRoleStore<Role,String>  store)
            : base(store) { }

        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> factory, IOwinContext context)
        {
            var dbcontext = context.Get<ApplicationDbContext>();
            AppRoleManager roleMgr =  new AppRoleManager(new RoleStore<Role>(dbcontext));
            return roleMgr;
        }

        public static AppRoleManager Create(ApplicationDbContext context)
        {
            AppRoleManager roleMgr = new AppRoleManager(new RoleStore<Role>(context));
            return roleMgr;
        }
    }
}
