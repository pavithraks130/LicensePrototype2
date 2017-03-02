namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubscriptionTypeTableChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartItem", "Price", c => c.Double(nullable: false));
            AddColumn("dbo.CartItem", "TotalPrice", c => c.Double(nullable: false));
            AddColumn("dbo.SubscriptionType", "ImagePath", c => c.String());
            DropColumn("dbo.Product", "Price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Product", "Price", c => c.String());
            DropColumn("dbo.SubscriptionType", "ImagePath");
            DropColumn("dbo.CartItem", "TotalPrice");
            DropColumn("dbo.CartItem", "Price");
        }
    }
}
