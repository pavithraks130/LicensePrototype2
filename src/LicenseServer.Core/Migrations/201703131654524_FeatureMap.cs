namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeatureMap : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Feature",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserToken",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Token = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductFeature",
                c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        Feature_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_Id, t.Feature_Id })
                .ForeignKey("dbo.Product", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.Feature", t => t.Feature_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.Feature_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductFeature", "Feature_Id", "dbo.Feature");
            DropForeignKey("dbo.ProductFeature", "Product_Id", "dbo.Product");
            DropIndex("dbo.ProductFeature", new[] { "Feature_Id" });
            DropIndex("dbo.ProductFeature", new[] { "Product_Id" });
            DropTable("dbo.ProductFeature");
            DropTable("dbo.UserToken");
            DropTable("dbo.Feature");
        }
    }
}
