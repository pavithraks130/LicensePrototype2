namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesToTeamLicense1 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.UserLicense", "TeamLicenseId");
            AddForeignKey("dbo.UserLicense", "TeamLicenseId", "dbo.TeamLicense", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserLicense", "TeamLicenseId", "dbo.TeamLicense");
            DropIndex("dbo.UserLicense", new[] { "TeamLicenseId" });
        }
    }
}
