namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProductAdditionalOptionTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ProductFeature", newName: "FeatureProduct");
            DropPrimaryKey("dbo.FeatureProduct");
            CreateTable(
                "dbo.ProductAdditionalOption",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(unicode: false, storeType: "text"),
                        ValueType = c.String(),
                        ProductId = c.Int(nullable: false),
                        SubscriptionCategory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.SubscriptionCategory", t => t.SubscriptionCategory_Id)
                .Index(t => t.ProductId)
                .Index(t => t.SubscriptionCategory_Id);
            
            AddPrimaryKey("dbo.FeatureProduct", new[] { "Feature_Id", "Product_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductAdditionalOption", "SubscriptionCategory_Id", "dbo.SubscriptionCategory");
            DropForeignKey("dbo.ProductAdditionalOption", "ProductId", "dbo.Product");
            DropIndex("dbo.ProductAdditionalOption", new[] { "SubscriptionCategory_Id" });
            DropIndex("dbo.ProductAdditionalOption", new[] { "ProductId" });
            DropPrimaryKey("dbo.FeatureProduct");
            DropTable("dbo.ProductAdditionalOption");
            AddPrimaryKey("dbo.FeatureProduct", new[] { "Product_Id", "Feature_Id" });
            RenameTable(name: "dbo.FeatureProduct", newName: "ProductFeature");
        }
    }
}
