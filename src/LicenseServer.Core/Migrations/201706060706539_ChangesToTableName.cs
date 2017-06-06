namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesToTableName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SubscriptionType", newName: "Subscription");
            RenameTable(name: "dbo.ProductCategory", newName: "SubscriptionCategory");
            RenameTable(name: "dbo.ProductProductCategory", newName: "ProductSubscriptionCategory");
            RenameColumn(table: "dbo.CartItem", name: "SubscriptionTypeId", newName: "SubscriptionId");
            RenameColumn(table: "dbo.ProductSubscriptionCategory", name: "ProductCategory_Id", newName: "SubscriptionCategory_Id");
            RenameColumn(table: "dbo.UserSubscription", name: "SubscriptionTypeId", newName: "SubscriptionId");
            RenameColumn(table: "dbo.SubscriptionDetail", name: "SubscriptionTypeId", newName: "SubscriptionId");
            RenameIndex(table: "dbo.CartItem", name: "IX_SubscriptionTypeId", newName: "IX_SubscriptionId");
            RenameIndex(table: "dbo.SubscriptionDetail", name: "IX_SubscriptionTypeId", newName: "IX_SubscriptionId");
            RenameIndex(table: "dbo.UserSubscription", name: "IX_SubscriptionTypeId", newName: "IX_SubscriptionId");
            RenameIndex(table: "dbo.ProductSubscriptionCategory", name: "IX_ProductCategory_Id", newName: "IX_SubscriptionCategory_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ProductSubscriptionCategory", name: "IX_SubscriptionCategory_Id", newName: "IX_ProductCategory_Id");
            RenameIndex(table: "dbo.UserSubscription", name: "IX_SubscriptionId", newName: "IX_SubscriptionTypeId");
            RenameIndex(table: "dbo.SubscriptionDetail", name: "IX_SubscriptionId", newName: "IX_SubscriptionTypeId");
            RenameIndex(table: "dbo.CartItem", name: "IX_SubscriptionId", newName: "IX_SubscriptionTypeId");
            RenameColumn(table: "dbo.SubscriptionDetail", name: "SubscriptionId", newName: "SubscriptionTypeId");
            RenameColumn(table: "dbo.UserSubscription", name: "SubscriptionId", newName: "SubscriptionTypeId");
            RenameColumn(table: "dbo.ProductSubscriptionCategory", name: "SubscriptionCategory_Id", newName: "ProductCategory_Id");
            RenameColumn(table: "dbo.CartItem", name: "SubscriptionId", newName: "SubscriptionTypeId");
            RenameTable(name: "dbo.ProductSubscriptionCategory", newName: "ProductProductCategory");
            RenameTable(name: "dbo.SubscriptionCategory", newName: "ProductCategory");
            RenameTable(name: "dbo.Subscription", newName: "SubscriptionType");
        }
    }
}
