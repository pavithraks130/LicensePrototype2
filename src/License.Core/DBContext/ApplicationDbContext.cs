using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Core.Model;
using Microsoft.AspNet.Identity.EntityFramework;

namespace License.Core.DBContext
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext() : base("DBConnectionString")
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.AutoDetectChangesEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<AppUser>().Property(p => p.IsActive).IsConcurrencyToken();
            base.OnModelCreating(modelBuilder);

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<TeamMember> UserInvite { get; set; }

        //public DbSet<Product> Product { get; set; }

        //public DbSet<Subscription> Subscription { get; set; }

        //public DbSet<ProductSubscriptionMapping> ProductSubscriptionMapping { get; set; }

        public DbSet<UserSubscription> UserSubscription { get; set; }

        public DbSet<LicenseData> LicenseData { get; set; }

        public DbSet<UserLicense> UserLicense { get; set; }
        public DbSet<TeamLicense> TeamLicense { get; set; }

        public DbSet<UserLicenseRequest> UserLicenseRequest { get; set; }

        public DbSet<TeamAsset> TeamAsset { get; set; }
    }
}
