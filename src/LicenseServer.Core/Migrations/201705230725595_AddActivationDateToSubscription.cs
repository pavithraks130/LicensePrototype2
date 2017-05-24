namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActivationDateToSubscription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserSubscription", "ActivationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserSubscription", "ActivationDate");
        }
    }
}
