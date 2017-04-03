namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPurchaseOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PurchaseOrder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        PurchaseOrderNo = c.String(),
                        IsApproved = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        IsSynched = c.Boolean(nullable: false),
                        ApprovedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.PurchaseOrderItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubscriptionId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        PurchaseOrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PurchaseOrder", t => t.PurchaseOrderId, cascadeDelete: true)
                .ForeignKey("dbo.SubscriptionType", t => t.SubscriptionId, cascadeDelete: true)
                .Index(t => t.SubscriptionId)
                .Index(t => t.PurchaseOrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseOrder", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PurchaseOrderItem", "SubscriptionId", "dbo.SubscriptionType");
            DropForeignKey("dbo.PurchaseOrderItem", "PurchaseOrderId", "dbo.PurchaseOrder");
            DropIndex("dbo.PurchaseOrderItem", new[] { "PurchaseOrderId" });
            DropIndex("dbo.PurchaseOrderItem", new[] { "SubscriptionId" });
            DropIndex("dbo.PurchaseOrder", new[] { "UserId" });
            DropTable("dbo.PurchaseOrderItem");
            DropTable("dbo.PurchaseOrder");
        }
    }
}
