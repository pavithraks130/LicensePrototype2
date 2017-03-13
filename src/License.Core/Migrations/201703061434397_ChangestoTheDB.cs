namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangestoTheDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserSubscription", "Quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserSubscription", "Quantity");
        }
    }
}
