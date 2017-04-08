namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init482017 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TeamMembers", newName: "TeamMember");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.TeamMember", newName: "TeamMembers");
        }
    }
}
