namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Product", "CategoryID", "dbo.Category");
            DropForeignKey("dbo.CartItem", "ProductId", "dbo.Product");
            DropIndex("dbo.Product", new[] { "CategoryID" });
            DropIndex("dbo.CartItem", new[] { "ProductId" });
            DropTable("dbo.Category");
            DropTable("dbo.Product");
            DropTable("dbo.CartItem");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CartItem",
                c => new
                    {
                        ItemId = c.String(nullable: false, maxLength: 128),
                        CartId = c.String(),
                        Quantity = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ItemId);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        ProductName = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false),
                        ImagePath = c.String(),
                        UnitPrice = c.Double(),
                        CategoryID = c.Int(),
                    })
                .PrimaryKey(t => t.ProductID);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateIndex("dbo.CartItem", "ProductId");
            CreateIndex("dbo.Product", "CategoryID");
            AddForeignKey("dbo.CartItem", "ProductId", "dbo.Product", "ProductID", cascadeDelete: true);
            AddForeignKey("dbo.Product", "CategoryID", "dbo.Category", "CategoryID");
        }
    }
}
