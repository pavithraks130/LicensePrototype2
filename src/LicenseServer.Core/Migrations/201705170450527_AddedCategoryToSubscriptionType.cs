namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCategoryToSubscriptionType : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ProductCategoryProduct", newName: "ProductProductCategory");
            DropPrimaryKey("dbo.ProductProductCategory");
            AddColumn("dbo.SubscriptionType", "CategoryId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ProductProductCategory", new[] { "Product_Id", "ProductCategory_Id" });
            CreateIndex("dbo.SubscriptionType", "CategoryId");
            AddForeignKey("dbo.SubscriptionType", "CategoryId", "dbo.ProductCategory", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubscriptionType", "CategoryId", "dbo.ProductCategory");
            DropIndex("dbo.SubscriptionType", new[] { "CategoryId" });
            DropPrimaryKey("dbo.ProductProductCategory");
            DropColumn("dbo.SubscriptionType", "CategoryId");
            AddPrimaryKey("dbo.ProductProductCategory", new[] { "ProductCategory_Id", "Product_Id" });
            RenameTable(name: "dbo.ProductProductCategory", newName: "ProductCategoryProduct");
        }
    }
}
