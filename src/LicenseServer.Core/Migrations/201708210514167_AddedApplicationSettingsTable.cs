namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedApplicationSettingsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientAppVerificationSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationCode = c.String(),
                        Keyword = c.String(),
                        ApplicationSecretkey = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ClientAppVerificationSettings");
        }
    }
}
