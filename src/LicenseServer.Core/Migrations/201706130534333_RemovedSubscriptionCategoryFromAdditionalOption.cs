namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedSubscriptionCategoryFromAdditionalOption : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.FeatureProduct", newName: "ProductFeature");
            DropForeignKey("dbo.ProductAdditionalOption", "SubscriptionCategory_Id", "dbo.SubscriptionCategory");
            DropIndex("dbo.ProductAdditionalOption", new[] { "SubscriptionCategory_Id" });
            DropPrimaryKey("dbo.ProductFeature");
            AddPrimaryKey("dbo.ProductFeature", new[] { "Product_Id", "Feature_Id" });
            DropColumn("dbo.ProductAdditionalOption", "SubscriptionCategory_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductAdditionalOption", "SubscriptionCategory_Id", c => c.Int());
            DropPrimaryKey("dbo.ProductFeature");
            AddPrimaryKey("dbo.ProductFeature", new[] { "Feature_Id", "Product_Id" });
            CreateIndex("dbo.ProductAdditionalOption", "SubscriptionCategory_Id");
            AddForeignKey("dbo.ProductAdditionalOption", "SubscriptionCategory_Id", "dbo.SubscriptionCategory", "Id");
            RenameTable(name: "dbo.ProductFeature", newName: "FeatureProduct");
        }
    }
}
