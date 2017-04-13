namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamTableChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Team", "IsDefaultTeam", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Team", "IsDefaultTeam");
        }
    }
}
