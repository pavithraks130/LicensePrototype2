namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCommentColumnToPOTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOrder", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseOrder", "Comment");
        }
    }
}
