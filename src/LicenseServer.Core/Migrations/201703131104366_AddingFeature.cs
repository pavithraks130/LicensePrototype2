namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingFeature : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.LicenseFeaturesProduct", newName: "ProductLicenseFeatures");
            DropPrimaryKey("dbo.ProductLicenseFeatures");
            AddPrimaryKey("dbo.ProductLicenseFeatures", new[] { "Product_Id", "LicenseFeatures_FeatureId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ProductLicenseFeatures");
            AddPrimaryKey("dbo.ProductLicenseFeatures", new[] { "LicenseFeatures_FeatureId", "Product_Id" });
            RenameTable(name: "dbo.ProductLicenseFeatures", newName: "LicenseFeaturesProduct");
        }
    }
}
