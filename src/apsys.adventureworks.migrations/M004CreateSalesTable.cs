using FluentMigrator;

namespace apsys.adventureworks.migrations
{

    [Migration(4)]
    public class M004CreateSalesTable : Migration
    {
        public override void Down()
        {
            Delete.Table("SalesOrderDetail");
            Delete.Table("SalesOrderHeader");
        }

        public override void Up()
        {
            Create.Table("SalesOrderHeader")
                .WithColumn("SalesOrderID").AsInt32().PrimaryKey()
                .WithColumn("RevisionNumber").AsInt32().NotNullable()
                .WithColumn("OrderDate").AsDateTime().NotNullable()
                .WithColumn("DueDate").AsDateTime().NotNullable()
                .WithColumn("ShipDate").AsDateTime().Nullable()
                .WithColumn("Status").AsInt32().NotNullable()
                .WithColumn("OnlineOrderFlag").AsBoolean().NotNullable()
                .WithColumn("SalesOrderNumber").AsString(25).NotNullable()
                .WithColumn("PurchaseOrderNumber").AsString(25).Nullable()
                .WithColumn("AccountNumber").AsString(15).Nullable()
                .WithColumn("CustomerID").AsInt32().ForeignKey("Customer", "CustomerID")
                .WithColumn("ShipToAddressID").AsInt32().ForeignKey("Address", "AddressId")
                .WithColumn("BillToAddressID").AsInt32().ForeignKey("Address", "AddressId")
                .WithColumn("ShipMethod").AsString(50).NotNullable()
                .WithColumn("CreditCardApprovalCode").AsString(15).Nullable()
                .WithColumn("SubTotal").AsDouble().NotNullable()
                .WithColumn("TaxAmt").AsDouble().NotNullable()
                .WithColumn("Freight").AsDouble().NotNullable()
                .WithColumn("TotalDue").AsDouble().NotNullable()
                .WithColumn("Comment").AsString().Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable();

            Create.Table("SalesOrderDetail")
                .WithColumn("SalesOrderDetailID").AsInt32().PrimaryKey()
                .WithColumn("SalesOrderID").AsInt32().NotNullable().ForeignKey("SalesOrderHeader", "SalesOrderID")
                .WithColumn("OrderQty").AsInt32().NotNullable()
                .WithColumn("ProductID").AsInt32().NotNullable().ForeignKey("Product","ProductID")
                .WithColumn("UnitPrice").AsDouble().NotNullable()
                .WithColumn("UnitPriceDiscount").AsDouble().NotNullable()
                .WithColumn("LineTotal").AsDouble().NotNullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable();
        }
    }
}
