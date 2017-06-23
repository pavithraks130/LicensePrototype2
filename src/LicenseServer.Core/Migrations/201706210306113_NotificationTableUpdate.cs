namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationTableUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notification", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notification", "Image");
        }
    }
}
