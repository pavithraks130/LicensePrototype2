namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManagerIdMappingtoUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ManagerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "ManagerId");
            AddForeignKey("dbo.AspNetUsers", "ManagerId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "ManagerId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "ManagerId" });
            DropColumn("dbo.AspNetUsers", "ManagerId");
        }
    }
}
