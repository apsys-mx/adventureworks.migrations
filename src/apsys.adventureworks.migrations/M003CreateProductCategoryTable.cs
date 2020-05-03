using FluentMigrator;

namespace apsys.adventureworks.migrations
{

    [Migration(3)]
    public class M003CreateProductCategoryTable : Migration
    {
        public override void Down()
        {
            Delete.Table("Product");
            Delete.Table("ProductModelProductDescription");
            Delete.Table("ProductModel");
            Delete.Table("ProductDescription");
            Delete.Table("ProductCategory");
        }

        public override void Up()
        {
            Create.Table("ProductCategory")
                .WithColumn("ProductCategoryID").AsInt32().PrimaryKey()
                .WithColumn("ParentProductCategoryID").AsInt32().ForeignKey("ProductCategory", "ProductCategoryID")
                .WithColumn("Name").AsString(50).NotNullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable();

            Create.Table("ProductDescription")
                .WithColumn("ProductDescriptionID").AsInt32().PrimaryKey()
                .WithColumn("Description").AsString().NotNullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable();

            Create.Table("ProductModel")
                .WithColumn("ProductModelID").AsInt32().PrimaryKey()
                .WithColumn("Name").AsString(50).NotNullable()
                .WithColumn("CatalogDescription").AsString(50).Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable();

            Create.Table("ProductModelProductDescription")
                .WithColumn("ProductModelID").AsInt32().ForeignKey("ProductModel", "ProductModelID")
                .WithColumn("ProductDescriptionID").AsInt32().ForeignKey("ProductDescription", "ProductDescriptionID")
                .WithColumn("Culture").AsString(6).NotNullable();
            Create.PrimaryKey("PK_ProductModelProductDescription")
                .OnTable("ProductModelProductDescription")
                .Columns("ProductModelID", "ProductDescriptionID", "Culture");

            Create.Table("Product")
                .WithColumn("ProductID").AsInt32().PrimaryKey()
                .WithColumn("Name").AsString(50).Unique().NotNullable()
                .WithColumn("ProductNumber").AsString(25).Unique().NotNullable()
                .WithColumn("Color").AsString(15).Nullable()
                .WithColumn("StandardCost").AsDouble().NotNullable()
                .WithColumn("ListPrice").AsDecimal().NotNullable()
                .WithColumn("Size").AsString(5).Nullable()
                .WithColumn("Weight").AsDecimal(8,2).Nullable()
                .WithColumn("ProductCategoryID").AsInt32().Nullable().ForeignKey("FK_Product_ProductCategory_ProductCategoryID", "ProductCategory", "ProductCategoryID")
                .WithColumn("ProductModelID").AsInt32().Nullable().ForeignKey("FK_Product_ProductModel_ProductModelID", "ProductModel", "ProductModelID")
                .WithColumn("SellStartDate").AsDateTime().NotNullable()
                .WithColumn("SellEndDate").AsDateTime().Nullable()
                .WithColumn("DiscontinuedDate").AsDateTime().Nullable()
                .WithColumn("ThumbNailPhoto").AsString().Nullable()
                .WithColumn("ThumbnailPhotoFileName").AsString(50).Nullable()
                .WithColumn("ModifiedDate").AsDateTime().NotNullable();
        }

    }
}
