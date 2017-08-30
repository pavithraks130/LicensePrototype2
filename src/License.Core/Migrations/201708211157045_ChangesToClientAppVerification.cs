namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesToClientAppVerification : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ClientAppVerificationSettings", "Keyword");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClientAppVerificationSettings", "Keyword", c => c.String());
        }
    }
}