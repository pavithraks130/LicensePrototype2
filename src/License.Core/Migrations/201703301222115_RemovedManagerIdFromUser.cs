namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedManagerIdFromUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "ManagerId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "ManagerId" });
            DropColumn("dbo.AspNetUsers", "ManagerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ManagerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "ManagerId");
            AddForeignKey("dbo.AspNetUsers", "ManagerId", "dbo.AspNetUsers", "Id");
        }
    }
}
