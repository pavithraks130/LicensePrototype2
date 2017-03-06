namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedSunscriptionTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LicenseData", "ProductId", "dbo.Product");
            DropForeignKey("dbo.UserSubscription", "SubscriptionId", "dbo.Subscription");
            DropForeignKey("dbo.ProductSubscriptionMapping", "ProductId", "dbo.Product");
            DropForeignKey("dbo.ProductSubscriptionMapping", "SubscriptionId", "dbo.Subscription");
            DropIndex("dbo.LicenseData", new[] { "ProductId" });
            DropIndex("dbo.UserSubscription", new[] { "SubscriptionId" });
            DropIndex("dbo.ProductSubscriptionMapping", new[] { "ProductId" });
            DropIndex("dbo.ProductSubscriptionMapping", new[] { "SubscriptionId" });
            DropTable("dbo.Product");
            DropTable("dbo.Subscription");
            DropTable("dbo.ProductSubscriptionMapping");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProductSubscriptionMapping",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        SubscriptionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subscription",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        SubscriptionName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        ProductCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.ProductSubscriptionMapping", "SubscriptionId");
            CreateIndex("dbo.ProductSubscriptionMapping", "ProductId");
            CreateIndex("dbo.UserSubscription", "SubscriptionId");
            CreateIndex("dbo.LicenseData", "ProductId");
            AddForeignKey("dbo.ProductSubscriptionMapping", "SubscriptionId", "dbo.Subscription", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProductSubscriptionMapping", "ProductId", "dbo.Product", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserSubscription", "SubscriptionId", "dbo.Subscription", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LicenseData", "ProductId", "dbo.Product", "Id", cascadeDelete: true);
        }
    }
}
