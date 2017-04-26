
using Microsoft.AspNet.Identity.EntityFramework;
using LicenseServer.Core.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace LicenseServer.Core.DbContext
{
    public class AppDbContext : IdentityDbContext<Appuser>
    {
        public AppDbContext() : base("LicenseDBConnectionString")
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.AutoDetectChangesEnabled = true;
        }
        public DbSet<Organization> Organization { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<CartItem> CartItem { get; set; }

        public DbSet<SubscriptionType> SubscriptionType { get; set; }

        public DbSet<SubscriptionDetail> SubscriptionDetail { get; set; }

        public DbSet<UserSubscription> UserSubScriptioin { get; set; }

        public DbSet<UserToken> UserToken { get; set; }

        public DbSet<Feature> Feature { get; set; }

        public DbSet<ProductCategory> ProductCategory { get; set; }

        public DbSet<PurchaseOrder> PurchaseOrder { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItem { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }
    }
}
