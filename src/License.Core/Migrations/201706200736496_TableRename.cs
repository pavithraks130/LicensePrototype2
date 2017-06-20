namespace License.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableRename : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VISMAData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TestDevice = c.String(),
                        ExpirationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VISMAData");
        }
    }
}
