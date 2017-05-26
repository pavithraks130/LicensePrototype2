namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamTableChange1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Team", "ConcurrentUserCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Team", "ConcurrentUserCount");
        }
    }
}
