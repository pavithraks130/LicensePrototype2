namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameTeamToOrganization : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Team", newName: "Organization");
            RenameColumn(table: "dbo.AspNetUsers", name: "TeamId", newName: "OrganizationId");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_TeamId", newName: "IX_OrganizationId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_OrganizationId", newName: "IX_TeamId");
            RenameColumn(table: "dbo.AspNetUsers", name: "OrganizationId", newName: "TeamId");
            RenameTable(name: "dbo.Organization", newName: "Team");
        }
    }
}
