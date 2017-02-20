namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRole : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Role_Id", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUsers", new[] { "Role_Id" });
            DropColumn("dbo.AspNetUsers", "Role_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Role_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "Role_Id");
            AddForeignKey("dbo.AspNetUsers", "Role_Id", "dbo.AspNetRoles", "Id");
        }
    }
}
