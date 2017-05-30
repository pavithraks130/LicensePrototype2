namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesToUserLicense1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserLicense", "TeamLicenseId", "dbo.TeamLicense");
            DropIndex("dbo.UserLicense", new[] { "TeamLicenseId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.UserLicense", "TeamLicenseId");
            AddForeignKey("dbo.UserLicense", "TeamLicenseId", "dbo.TeamLicense", "Id", cascadeDelete: true);
        }
    }
}
