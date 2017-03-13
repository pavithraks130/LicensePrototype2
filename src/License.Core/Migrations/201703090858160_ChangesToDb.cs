namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesToDb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LicenseData", "IsMapped", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LicenseData", "IsMapped");
        }
    }
}
