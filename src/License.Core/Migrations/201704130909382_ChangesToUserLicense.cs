namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesToUserLicense : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserLicenseRequest", "TeamId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserLicenseRequest", "TeamId");
        }
    }
}
