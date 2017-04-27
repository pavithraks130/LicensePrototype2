namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeatureTableChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Feature", "IsEnabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Feature", "IsEnabled");
        }
    }
}
