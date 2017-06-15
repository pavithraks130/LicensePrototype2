namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedColumnToRoleTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetRoles", "IsDefault", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetRoles", "IsDefault");
        }
    }
}
