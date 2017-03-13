namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PriceColumnToCartItem : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CartItem", "TotalPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CartItem", "TotalPrice", c => c.Double(nullable: false));
        }
    }
}
