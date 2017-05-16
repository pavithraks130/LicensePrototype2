namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPriceToFeataure : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Feature", "price", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Feature", "price");
        }
    }
}
