using FluentMigrator;

namespace apsys.adventureworks.migrations
{

    [Migration(1)]
    public class M001CreateAddressTable : Migration
    {
        public override void Down()
        {
            Delete.Column("Address");
        }

        public override void Up()
        {
            Create.Table("Address")
                .WithColumn("AddressID").AsInt32().PrimaryKey().NotNullable()
                .WithColumn("AddressLine1").AsString(60).NotNullable()
                .WithColumn("AddressLine2").AsString(60).NotNullable()
                .WithColumn("City").AsString(30).NotNullable()
                .WithColumn("StateProvince").AsString(50).NotNullable()
                .WithColumn("CountryRegion").AsString(50).NotNullable()
                .WithColumn("PostalCode").AsString(15).NotNullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable();
        }
    }
}
