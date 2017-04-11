namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesToSubscription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubscriptionType", "CreatedBy", c => c.String(maxLength: 128));
            CreateIndex("dbo.SubscriptionType", "CreatedBy");
            AddForeignKey("dbo.SubscriptionType", "CreatedBy", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubscriptionType", "CreatedBy", "dbo.AspNetUsers");
            DropIndex("dbo.SubscriptionType", new[] { "CreatedBy" });
            DropColumn("dbo.SubscriptionType", "CreatedBy");
        }
    }
}
