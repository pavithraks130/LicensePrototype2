namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeIdTypeToCartItem : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.CartItem");
            AlterColumn("dbo.CartItem", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.CartItem", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.CartItem");
            AlterColumn("dbo.CartItem", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.CartItem", "Id");
        }
    }
}
