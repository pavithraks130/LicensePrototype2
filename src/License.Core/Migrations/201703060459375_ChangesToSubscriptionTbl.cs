namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesToSubscriptionTbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "ProductCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "ProductCode");
        }
    }
}
