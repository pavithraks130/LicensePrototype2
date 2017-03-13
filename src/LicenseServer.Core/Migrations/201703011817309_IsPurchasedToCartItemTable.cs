namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsPurchasedToCartItemTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartItem", "IsPurchased", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CartItem", "IsPurchased");
        }
    }
}
