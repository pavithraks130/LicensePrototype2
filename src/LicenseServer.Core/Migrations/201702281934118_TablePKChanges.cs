namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TablePKChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserSubscription", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.UserSubscription", "UserId");
            CreateIndex("dbo.UserSubscription", "SubscriptionTypeId");
            AddForeignKey("dbo.UserSubscription", "SubscriptionTypeId", "dbo.SubscriptionType", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserSubscription", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserSubscription", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserSubscription", "SubscriptionTypeId", "dbo.SubscriptionType");
            DropIndex("dbo.UserSubscription", new[] { "SubscriptionTypeId" });
            DropIndex("dbo.UserSubscription", new[] { "UserId" });
            AlterColumn("dbo.UserSubscription", "UserId", c => c.String());
        }
    }
}
