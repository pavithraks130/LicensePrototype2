using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using LicenseServer.Core.DbContext;
using LicenseServer.Core.Model;
using Microsoft.Owin;

namespace LicenseServer.Core.Manager
{
    public class LicRoleManager : RoleManager<AppRole>
    {
        public LicRoleManager(IRoleStore<AppRole, string> store)
            : base(store) { }

        public static LicRoleManager Create(IdentityFactoryOptions<LicRoleManager> factory, IOwinContext context)
        {
            var dbcontext = context.Get<AppDbContext>();
            LicRoleManager roleMgr = new LicRoleManager(new RoleStore<AppRole>(dbcontext));
            return roleMgr;
        }

        public static LicRoleManager Create(AppDbContext dbcontext)
        {
            LicRoleManager roleMgr = new LicRoleManager(new RoleStore<AppRole>(dbcontext));
            return roleMgr;
        }

    }
}
