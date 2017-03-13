namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeatureList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LicenseFeatures",
                c => new
                    {
                        FeatureId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.FeatureId);
            
            CreateTable(
                "dbo.LicenseFeaturesProduct",
                c => new
                    {
                        LicenseFeatures_FeatureId = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LicenseFeatures_FeatureId, t.Product_Id })
                .ForeignKey("dbo.LicenseFeatures", t => t.LicenseFeatures_FeatureId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.LicenseFeatures_FeatureId)
                .Index(t => t.Product_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LicenseFeaturesProduct", "Product_Id", "dbo.Product");
            DropForeignKey("dbo.LicenseFeaturesProduct", "LicenseFeatures_FeatureId", "dbo.LicenseFeatures");
            DropIndex("dbo.LicenseFeaturesProduct", new[] { "Product_Id" });
            DropIndex("dbo.LicenseFeaturesProduct", new[] { "LicenseFeatures_FeatureId" });
            DropTable("dbo.LicenseFeaturesProduct");
            DropTable("dbo.LicenseFeatures");
        }
    }
}
