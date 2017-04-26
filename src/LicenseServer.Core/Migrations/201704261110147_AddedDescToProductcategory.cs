namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDescToProductcategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductCategory", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductCategory", "Description");
        }
    }
}
