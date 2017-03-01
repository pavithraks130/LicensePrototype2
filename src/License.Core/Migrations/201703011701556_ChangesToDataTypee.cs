namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesToDataTypee : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserSubscription", "SubscriptionId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserSubscription", "SubscriptionId", c => c.String());
        }
    }
}
