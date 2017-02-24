namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamMembersandOrganizationchanges : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Teams", newName: "Team");
            CreateTable(
                "dbo.TeamMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdminId = c.String(maxLength: 128),
                        TeamId = c.Int(nullable: false),
                        InviteeEmail = c.String(),
                        InviteeUserId = c.String(maxLength: 128),
                        InviteeStatus = c.String(),
                        InvitationDate = c.DateTime(nullable: false),
                        IsAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AdminId)
                .ForeignKey("dbo.AspNetUsers", t => t.InviteeUserId)
                .Index(t => t.AdminId)
                .Index(t => t.InviteeUserId);
            
            CreateIndex("dbo.AspNetUsers", "TeamId");
            AddForeignKey("dbo.AspNetUsers", "TeamId", "dbo.Team", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamMembers", "InviteeUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TeamMembers", "AdminId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "TeamId", "dbo.Team");
            DropIndex("dbo.AspNetUsers", new[] { "TeamId" });
            DropIndex("dbo.TeamMembers", new[] { "InviteeUserId" });
            DropIndex("dbo.TeamMembers", new[] { "AdminId" });
            DropTable("dbo.TeamMembers");
            RenameTable(name: "dbo.Team", newName: "Teams");
        }
    }
}
