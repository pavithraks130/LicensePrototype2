namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelPropertychanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CSVFile", "TestDevice", c => c.String());
            AddColumn("dbo.CSVFile", "ExpirationDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.CSVFile", "SerialNumber");
            DropColumn("dbo.CSVFile", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CSVFile", "Description", c => c.String());
            AddColumn("dbo.CSVFile", "SerialNumber", c => c.String());
            DropColumn("dbo.CSVFile", "ExpirationDate");
            DropColumn("dbo.CSVFile", "TestDevice");
        }
    }
}
