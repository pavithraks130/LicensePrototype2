namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.LicenseData", name: "SubscriptionId", newName: "UserSubscriptionId");
            RenameIndex(table: "dbo.LicenseData", name: "IX_SubscriptionId", newName: "IX_UserSubscriptionId");
            AddColumn("dbo.LicenseData", "ProductId", c => c.Int(nullable: false));
            AddColumn("dbo.LicenseData", "IsMapped", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserSubscription", "Quantity", c => c.Int(nullable: false));
            DropColumn("dbo.LicenseData", "AdminUserId");
            DropColumn("dbo.UserSubscription", "ServerUserId");
            DropColumn("dbo.UserSubscription", "SubscriptionName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserSubscription", "SubscriptionName", c => c.String());
            AddColumn("dbo.UserSubscription", "ServerUserId", c => c.String());
            AddColumn("dbo.LicenseData", "AdminUserId", c => c.String());
            DropColumn("dbo.UserSubscription", "Quantity");
            DropColumn("dbo.LicenseData", "IsMapped");
            DropColumn("dbo.LicenseData", "ProductId");
            RenameIndex(table: "dbo.LicenseData", name: "IX_UserSubscriptionId", newName: "IX_SubscriptionId");
            RenameColumn(table: "dbo.LicenseData", name: "UserSubscriptionId", newName: "SubscriptionId");
        }
    }
}
