namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBConfig2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "Price", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "Price");
        }
    }
}
