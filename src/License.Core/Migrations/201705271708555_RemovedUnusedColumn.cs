namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedUnusedColumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.LicenseData", "TeamSubscriptionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LicenseData", "TeamSubscriptionId", c => c.Int(nullable: false));
        }
    }
}
