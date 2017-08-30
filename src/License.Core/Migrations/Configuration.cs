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
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(License.Core.DBContext.ApplicationDbContext context)
        {
            var isDbInitialized = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("IsDbIntialize"));
            if (isDbInitialized)
            {
                var roleManager = AppRoleManager.Create(context);
                if (!roleManager.RoleExists("Super Admin"))
                    roleManager.Create(new Role() { Name = "Super Admin" ,IsDefault = true});

                if (!roleManager.RoleExists("Tenant Admin"))
                    roleManager.Create(new Role() { Name = "Tenant Admin" });

                if (!roleManager.RoleExists("Application Admin"))
                    roleManager.Create(new Role() { Name = "Application Admin" });

                if (!roleManager.RoleExists("Author"))
                    roleManager.Create(new Role() { Name = "Author" });

                if (!roleManager.RoleExists("Approver"))
                    roleManager.Create(new Role() { Name = "Approver" });

                if (!roleManager.RoleExists("User"))
                    roleManager.Create(new Role() { Name = "User", IsDefault = true });

                if (!roleManager.RoleExists("Tenant"))
                    roleManager.Create(new Role() { Name = "Tenant", IsDefault = true });
            }

        }
    }
}
