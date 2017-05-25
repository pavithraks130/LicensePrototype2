namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserSubscriptionColumnChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserSubscription", "ExpireDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserSubscription", "RenewalDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.UserSubscription", "SubscriptionDate");
            DropColumn("dbo.UserSubscription", "ActiveDurataion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserSubscription", "ActiveDurataion", c => c.Int(nullable: false));
            AddColumn("dbo.UserSubscription", "SubscriptionDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.UserSubscription", "RenewalDate");
            DropColumn("dbo.UserSubscription", "ExpireDate");
        }
    }
}
