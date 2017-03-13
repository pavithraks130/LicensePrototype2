namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PriceColumnToSubscriptionType : DbMigration
    {
        public override void Up()
        {
            
            AddColumn("dbo.SubscriptionType", "Price", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SubscriptionType", "Price");
            
        }
    }
}
