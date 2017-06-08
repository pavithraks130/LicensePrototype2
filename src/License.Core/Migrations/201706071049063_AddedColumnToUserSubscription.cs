namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedColumnToUserSubscription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserSubscription", "ServerUserSubscriptionId", c => c.Int(nullable: false));
            AddColumn("dbo.UserSubscription", "ExpireDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserSubscription", "ExpireDate");
            DropColumn("dbo.UserSubscription", "ServerUserSubscriptionId");
        }
    }
}
