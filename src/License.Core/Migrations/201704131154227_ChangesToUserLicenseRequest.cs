namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesToUserLicenseRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserLicense", "TeamId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserLicense", "TeamId");
        }
    }
}
