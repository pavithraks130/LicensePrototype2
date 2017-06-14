namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUserSubscriptionTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserLicenseRequest", "UserSubscriptionId", "dbo.UserSubscription");
            DropIndex("dbo.UserLicenseRequest", new[] { "UserSubscriptionId" });
            DropColumn("dbo.UserLicenseRequest", "UserSubscriptionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserLicenseRequest", "UserSubscriptionId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserLicenseRequest", "UserSubscriptionId");
            AddForeignKey("dbo.UserLicenseRequest", "UserSubscriptionId", "dbo.UserSubscription", "Id", cascadeDelete: true);
        }
    }
}
