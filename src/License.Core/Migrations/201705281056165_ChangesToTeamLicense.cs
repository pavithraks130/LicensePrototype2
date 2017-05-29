namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesToTeamLicense : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeamLicense", "ProductId", c => c.Int(nullable: false));
            AddColumn("dbo.UserLicense", "TeamLicenseId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserLicense", "TeamLicenseId");
            AddForeignKey("dbo.UserLicense", "TeamLicenseId", "dbo.TeamLicense", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserLicense", "TeamLicenseId", "dbo.TeamLicense");
            DropIndex("dbo.UserLicense", new[] { "TeamLicenseId" });
            DropColumn("dbo.UserLicense", "TeamLicenseId");
            DropColumn("dbo.TeamLicense", "ProductId");
        }
    }
}
