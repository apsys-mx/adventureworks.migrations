using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace apsys.adventureworks.migrations
{

    [Migration(2)]
    public class M002CreateCustomersTable : Migration
    {
        public override void Down()
        {
            Delete.Table("CustomerAddress");
            Delete.Table("Customer");
        }

        public override void Up()
        {
            Create.Table("Customer")
                .WithColumn("CustomerID").AsInt32().PrimaryKey()
                .WithColumn("NameStyle").AsBoolean().NotNullable()
                .WithColumn("Title").AsString(8).Nullable()
                .WithColumn("FirstName").AsString(50).NotNullable()
                .WithColumn("MiddleName").AsString(50).Nullable()
                .WithColumn("LastName").AsString(50).NotNullable()
                .WithColumn("Suffix").AsString(10).Nullable()
                .WithColumn("CompanyName").AsString(128).Nullable()
                .WithColumn("SalesPerson").AsString(256).Nullable()
                .WithColumn("EmailAddress").AsString(50).Nullable()
                .WithColumn("Phone").AsString(25).Nullable()
                .WithColumn("PasswordHash").AsString(128).NotNullable()
                .WithColumn("PasswordSalt").AsString(10).NotNullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable();

            Create.Table("CustomerAddress")
                .WithColumn("CustomerID").AsInt32().ForeignKey("Customer", "CustomerID")
                .WithColumn("AddressID").AsInt32().ForeignKey("Address", "AddressID")
                .WithColumn("AddressType").AsString(50).NotNullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable();

        }
    }
}
