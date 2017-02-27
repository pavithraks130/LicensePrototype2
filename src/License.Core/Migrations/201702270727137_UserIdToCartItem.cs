namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIdToCartItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartItem", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.CartItem", "UserId");
            AddForeignKey("dbo.CartItem", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CartItem", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.CartItem", new[] { "UserId" });
            DropColumn("dbo.CartItem", "UserId");
        }
    }
}
