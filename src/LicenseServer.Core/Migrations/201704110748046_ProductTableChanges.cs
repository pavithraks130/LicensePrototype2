namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductTableChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "Price", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "Price");
        }
    }
}
