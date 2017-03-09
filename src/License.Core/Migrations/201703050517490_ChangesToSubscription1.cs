namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesToSubscription1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.LicenseData", name: "SubscriptionId", newName: "UserSubscriptionId");
            RenameIndex(table: "dbo.LicenseData", name: "IX_SubscriptionId", newName: "IX_UserSubscriptionId");
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
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
                "dbo.ProductSubscriptionMapping",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        SubscriptionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Subscription", t => t.SubscriptionId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.SubscriptionId);
            
            AddColumn("dbo.LicenseData", "ProductId", c => c.Int(nullable: false));
            CreateIndex("dbo.LicenseData", "ProductId");
            CreateIndex("dbo.UserSubscription", "SubscriptionId");
            AddForeignKey("dbo.LicenseData", "ProductId", "dbo.Product", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserSubscription", "SubscriptionId", "dbo.Subscription", "Id", cascadeDelete: true);
            DropColumn("dbo.LicenseData", "AdminUserId");
            DropColumn("dbo.UserSubscription", "ServerUserId");
            DropColumn("dbo.UserSubscription", "SubscriptionName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserSubscription", "SubscriptionName", c => c.String());
            AddColumn("dbo.UserSubscription", "ServerUserId", c => c.String());
            AddColumn("dbo.LicenseData", "AdminUserId", c => c.String());
            DropForeignKey("dbo.ProductSubscriptionMapping", "SubscriptionId", "dbo.Subscription");
            DropForeignKey("dbo.ProductSubscriptionMapping", "ProductId", "dbo.Product");
            DropForeignKey("dbo.UserSubscription", "SubscriptionId", "dbo.Subscription");
            DropForeignKey("dbo.LicenseData", "ProductId", "dbo.Product");
            DropIndex("dbo.ProductSubscriptionMapping", new[] { "SubscriptionId" });
            DropIndex("dbo.ProductSubscriptionMapping", new[] { "ProductId" });
            DropIndex("dbo.UserSubscription", new[] { "SubscriptionId" });
            DropIndex("dbo.LicenseData", new[] { "ProductId" });
            DropColumn("dbo.LicenseData", "ProductId");
            DropTable("dbo.ProductSubscriptionMapping");
            DropTable("dbo.Subscription");
            DropTable("dbo.Product");
            RenameIndex(table: "dbo.LicenseData", name: "IX_UserSubscriptionId", newName: "IX_SubscriptionId");
            RenameColumn(table: "dbo.LicenseData", name: "UserSubscriptionId", newName: "SubscriptionId");
        }
    }
}
