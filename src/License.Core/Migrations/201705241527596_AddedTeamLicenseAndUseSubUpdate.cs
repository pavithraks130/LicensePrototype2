namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTeamLicenseAndUseSubUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamLicense",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LicenseId = c.Int(nullable: false),
                        TeamId = c.Int(nullable: false),
                        IsMapped = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LicenseData", t => t.LicenseId, cascadeDelete: true)
                .Index(t => t.LicenseId);
            
            AddColumn("dbo.LicenseData", "TeamSubscriptionId", c => c.Int(nullable: false));
            AddColumn("dbo.UserSubscription", "RenewalDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserLicense", "IsTeamLicense", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamLicense", "LicenseId", "dbo.LicenseData");
            DropIndex("dbo.TeamLicense", new[] { "LicenseId" });
            DropColumn("dbo.UserLicense", "IsTeamLicense");
            DropColumn("dbo.UserSubscription", "RenewalDate");
            DropColumn("dbo.LicenseData", "TeamSubscriptionId");
            DropTable("dbo.TeamLicense");
        }
    }
}
