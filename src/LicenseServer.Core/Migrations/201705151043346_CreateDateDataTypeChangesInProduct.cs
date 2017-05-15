namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDateDataTypeChangesInProduct : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Product", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Product", "CreatedDate", c => c.String());
        }
    }
}
