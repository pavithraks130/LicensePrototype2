namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userLicenseRequest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserLicenseRequest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Requested_UserId = c.String(maxLength: 128),
                        UserSubscriptionId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        RequestedDate = c.DateTime(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Requested_UserId)
                .ForeignKey("dbo.UserSubscription", t => t.UserSubscriptionId, cascadeDelete: true)
                .Index(t => t.Requested_UserId)
                .Index(t => t.UserSubscriptionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserLicenseRequest", "UserSubscriptionId", "dbo.UserSubscription");
            DropForeignKey("dbo.UserLicenseRequest", "Requested_UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserLicenseRequest", new[] { "UserSubscriptionId" });
            DropIndex("dbo.UserLicenseRequest", new[] { "Requested_UserId" });
            DropTable("dbo.UserLicenseRequest");
        }
    }
}
