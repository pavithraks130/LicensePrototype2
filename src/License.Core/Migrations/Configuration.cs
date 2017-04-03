namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using License.Core.Manager;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using License.Core.Model;
    using License.Core.DBContext;

    internal sealed class Configuration : DbMigrationsConfiguration<License.Core.DBContext.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(License.Core.DBContext.ApplicationDbContext context)
        {
            //bool adminRecordRequired = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("IsAdminRecordReq"));
            //string adminUserName = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("AdminUserName"));
            //string adminPassword = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("AdminPassword"));
            //if (adminRecordRequired)
            //{
            //    var userManager = new UserManager<AppUser>(new UserStore<AppUser>(new ApplicationDbContext()));
            //    var roleManager = new RoleManager<Role>(new RoleStore<Role>(new ApplicationDbContext()));
            //    string roleName = "SuperAdminn";
            //    if (!context.Roles.Any(r => r.Name == roleName))
            //        roleManager.Create(new Role() { Name = roleName });
            //    var user = userManager.FindByEmail(adminUserName);
            //    if (user != null && !userManager.IsInRole(user.UserId, roleName))
            //        userManager.AddToRole(user.Id, roleName);
            //    else
            //    {
            //        user = new AppUser() { Email = adminUserName, UserName = adminUserName , OrganizationId = 1};
            //        userManager.Create(user, adminPassword);
            //        userManager.AddToRole(user.Id, roleName);
            //    }
            //}
        }
    }
}
