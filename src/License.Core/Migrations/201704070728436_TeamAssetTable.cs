namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamAssetTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamAsset",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdminId = c.String(),
                        TeamId = c.Int(nullable: false),
                        Name = c.String(),
                        SerialNumber = c.String(),
                        Type = c.String(),
                        Model = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TeamAsset");
        }
    }
}
