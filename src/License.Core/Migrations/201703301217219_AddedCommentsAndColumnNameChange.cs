namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCommentsAndColumnNameChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserLicenseRequest", "ApprovedBy", c => c.String());
            AddColumn("dbo.UserLicenseRequest", "Comment", c => c.String());
            DropColumn("dbo.UserLicenseRequest", "ModifiedBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserLicenseRequest", "ModifiedBy", c => c.String());
            DropColumn("dbo.UserLicenseRequest", "Comment");
            DropColumn("dbo.UserLicenseRequest", "ApprovedBy");
        }
    }
}
