namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserTeamId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Team_Id", "dbo.Teams");
            DropIndex("dbo.AspNetUsers", new[] { "Team_Id" });
            AddColumn("dbo.AspNetUsers", "TeamId", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "ManagerId");
            DropColumn("dbo.AspNetUsers", "Team_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Team_Id", c => c.Int());
            AddColumn("dbo.AspNetUsers", "ManagerId", c => c.String());
            DropColumn("dbo.AspNetUsers", "TeamId");
            CreateIndex("dbo.AspNetUsers", "Team_Id");
            AddForeignKey("dbo.AspNetUsers", "Team_Id", "dbo.Teams", "Id");
        }
    }
}
