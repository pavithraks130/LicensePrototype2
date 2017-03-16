namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VersionToFeature : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Feature", "Version", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Feature", "Version");
        }
    }
}
