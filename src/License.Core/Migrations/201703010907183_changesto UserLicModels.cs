namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changestoUserLicModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LicenseData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdminUserId = c.String(),
                        LicenseKey = c.String(),
                        SubscriptionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserSubscription", t => t.SubscriptionId, cascadeDelete: true)
                .Index(t => t.SubscriptionId);
            
            CreateTable(
                "dbo.UserSubscription",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        ServerUserId = c.String(),
                        SubscriptionId = c.String(),
                        SubscriptionName = c.String(),
                        SubscriptionDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLicense",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        LicenseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LicenseData", t => t.LicenseId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.LicenseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserLicense", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserLicense", "LicenseId", "dbo.LicenseData");
            DropForeignKey("dbo.LicenseData", "SubscriptionId", "dbo.UserSubscription");
            DropForeignKey("dbo.UserSubscription", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserLicense", new[] { "LicenseId" });
            DropIndex("dbo.UserLicense", new[] { "UserId" });
            DropIndex("dbo.UserSubscription", new[] { "UserId" });
            DropIndex("dbo.LicenseData", new[] { "SubscriptionId" });
            DropTable("dbo.UserLicense");
            DropTable("dbo.UserSubscription");
            DropTable("dbo.LicenseData");
        }
    }
}
