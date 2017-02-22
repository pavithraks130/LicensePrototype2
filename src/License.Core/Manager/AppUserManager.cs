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
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> userStore)
            : base(userStore) { }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> factory, IOwinContext context)
        {
            var dbcontext = context.Get<ApplicationDbContext>();
            var usermanager = new AppUserManager(new UserStore<AppUser>(dbcontext));
            // allow alphanumeric characters in username
            usermanager.UserValidator = new UserValidator<AppUser>(usermanager)
            {
                AllowOnlyAlphanumericUserNames = false
            };

            usermanager.EmailService = new EmailService();
            usermanager.ClaimsIdentityFactory = new ClaimsIdentityFactory<AppUser>();
            return usermanager;
        }
    }
}
