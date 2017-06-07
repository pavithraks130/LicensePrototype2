namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CHangesToTableName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.LicenseData", newName: "ProductLicense");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ProductLicense", newName: "LicenseData");
        }
    }
}
