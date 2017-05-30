namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesToTeamLicense : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeamLicense", "ProductId", c => c.Int(nullable: true));
            AddColumn("dbo.UserLicense", "TeamLicenseId", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserLicense", "TeamLicenseId");
            DropColumn("dbo.TeamLicense", "ProductId");
        }
    }
}
