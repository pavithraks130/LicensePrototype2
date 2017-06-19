namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNotificationTableEdit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notification", "NotificationData", c => c.String());
            DropColumn("dbo.Notification", "NotifiacationData");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notification", "NotifiacationData", c => c.String());
            DropColumn("dbo.Notification", "NotificationData");
        }
    }
}
