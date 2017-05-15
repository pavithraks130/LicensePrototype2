namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesToProductAndCategoryAndFeatureForColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Feature", "Caategory_Id", c => c.Int());
            AddColumn("dbo.Product", "ModifiedDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Feature", "Caategory_Id");
            AddForeignKey("dbo.Feature", "Caategory_Id", "dbo.ProductCategory", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Feature", "Caategory_Id", "dbo.ProductCategory");
            DropIndex("dbo.Feature", new[] { "Caategory_Id" });
            DropColumn("dbo.Product", "ModifiedDate");
            DropColumn("dbo.Feature", "Caategory_Id");
        }
    }
}
