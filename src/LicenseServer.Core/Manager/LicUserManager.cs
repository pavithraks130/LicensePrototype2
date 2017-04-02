using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using LicenseServer.Core.Model;
using LicenseServer.Core.DbContext;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace LicenseServer.Core.Manager
{
    public class LicUserManager : UserManager<Appuser>
    {
        public LicUserManager(IUserStore<Appuser> userStore)
            : base(userStore) { }

        public static LicUserManager Create(AppDbContext context)
        {
            var dbcontext = context;
            var usermanager = new LicUserManager(new UserStore<Appuser>(dbcontext));
            var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("LicenseProtoType");
            usermanager.UserTokenProvider = new Microsoft.AspNet.Identity.Owin.DataProtectorTokenProvider<Appuser>(provider.Create("EmailConfirmation"));
            return usermanager;
        }

        public static LicUserManager Create(IdentityFactoryOptions<LicUserManager> userManager, IOwinContext context)
        {
            var dbContext = context.Get<AppDbContext>();
            var userSstore = new UserStore<Appuser>(dbContext);
            var userMgr = new LicUserManager(userSstore);
            return userMgr;
        }
    }
}
