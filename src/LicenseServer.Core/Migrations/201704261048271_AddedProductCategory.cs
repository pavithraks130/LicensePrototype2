namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProductCategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductCategoryProduct",
                c => new
                    {
                        ProductCategory_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductCategory_Id, t.Product_Id })
                .ForeignKey("dbo.ProductCategory", t => t.ProductCategory_Id, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.ProductCategory_Id)
                .Index(t => t.Product_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductCategoryProduct", "Product_Id", "dbo.Product");
            DropForeignKey("dbo.ProductCategoryProduct", "ProductCategory_Id", "dbo.ProductCategory");
            DropIndex("dbo.ProductCategoryProduct", new[] { "Product_Id" });
            DropIndex("dbo.ProductCategoryProduct", new[] { "ProductCategory_Id" });
            DropTable("dbo.ProductCategoryProduct");
            DropTable("dbo.ProductCategory");
        }
    }
}
