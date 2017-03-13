namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.LicenseFeatures", newName: "Features");
            RenameTable(name: "dbo.ProductLicenseFeatures", newName: "ProductFeatures");
            RenameColumn(table: "dbo.ProductFeatures", name: "LicenseFeatures_FeatureId", newName: "Features_FeatureId");
            RenameIndex(table: "dbo.ProductFeatures", name: "IX_LicenseFeatures_FeatureId", newName: "IX_Features_FeatureId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ProductFeatures", name: "IX_Features_FeatureId", newName: "IX_LicenseFeatures_FeatureId");
            RenameColumn(table: "dbo.ProductFeatures", name: "Features_FeatureId", newName: "LicenseFeatures_FeatureId");
            RenameTable(name: "dbo.ProductFeatures", newName: "ProductLicenseFeatures");
            RenameTable(name: "dbo.Features", newName: "LicenseFeatures");
        }
    }
}
