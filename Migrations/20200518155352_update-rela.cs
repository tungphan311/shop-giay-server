using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace shop_giay_server.Migrations
{
    public partial class updaterela : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShoesBrandId",
                table: "Shoes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShoesTypeId",
                table: "Shoes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Payments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderId1",
                table: "Payments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Provider",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provider", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ColorId",
                table: "Stocks",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ShoesId",
                table: "Stocks",
                column: "ShoesId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_SizeId",
                table: "Stocks",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoesImages_ShoesId",
                table: "ShoesImages",
                column: "ShoesId");

            migrationBuilder.CreateIndex(
                name: "IX_Shoes_GenderId",
                table: "Shoes",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Shoes_ShoesBrandId",
                table: "Shoes",
                column: "ShoesBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Shoes_ShoesTypeId",
                table: "Shoes",
                column: "ShoesTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleProducts_SaleId",
                table: "SaleProducts",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleProducts_StockId",
                table: "SaleProducts",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId1",
                table: "Payments",
                column: "OrderId1");

            migrationBuilder.CreateIndex(
                name: "IX_Imports_ProviderId",
                table: "Imports",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportDetails_ImportId",
                table: "ImportDetails",
                column: "ImportId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportDetails_StockId",
                table: "ImportDetails",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReviews_ShoesId",
                table: "CustomerReviews",
                column: "ShoesId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_StockId",
                table: "CartItems",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresss_CustomerId",
                table: "Addresss",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresss_Customers_CustomerId",
                table: "Addresss",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartId",
                table: "CartItems",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Stocks_StockId",
                table: "CartItems",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerReviews_Shoes_ShoesId",
                table: "CustomerReviews",
                column: "ShoesId",
                principalTable: "Shoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportDetails_Imports_ImportId",
                table: "ImportDetails",
                column: "ImportId",
                principalTable: "Imports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportDetails_Stocks_StockId",
                table: "ImportDetails",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Imports_Provider_ProviderId",
                table: "Imports",
                column: "ProviderId",
                principalTable: "Provider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Orders_OrderId1",
                table: "Payments",
                column: "OrderId1",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleProducts_Sales_SaleId",
                table: "SaleProducts",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleProducts_Stocks_StockId",
                table: "SaleProducts",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shoes_Genders_GenderId",
                table: "Shoes",
                column: "GenderId",
                principalTable: "Genders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shoes_ShoesBrands_ShoesBrandId",
                table: "Shoes",
                column: "ShoesBrandId",
                principalTable: "ShoesBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shoes_ShoesTypes_ShoesTypeId",
                table: "Shoes",
                column: "ShoesTypeId",
                principalTable: "ShoesTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoesImages_Shoes_ShoesId",
                table: "ShoesImages",
                column: "ShoesId",
                principalTable: "Shoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Colors_ColorId",
                table: "Stocks",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Shoes_ShoesId",
                table: "Stocks",
                column: "ShoesId",
                principalTable: "Shoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Sizes_SizeId",
                table: "Stocks",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresss_Customers_CustomerId",
                table: "Addresss");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Stocks_StockId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerReviews_Shoes_ShoesId",
                table: "CustomerReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportDetails_Imports_ImportId",
                table: "ImportDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportDetails_Stocks_StockId",
                table: "ImportDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Imports_Provider_ProviderId",
                table: "Imports");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Orders_OrderId1",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleProducts_Sales_SaleId",
                table: "SaleProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleProducts_Stocks_StockId",
                table: "SaleProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shoes_Genders_GenderId",
                table: "Shoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Shoes_ShoesBrands_ShoesBrandId",
                table: "Shoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Shoes_ShoesTypes_ShoesTypeId",
                table: "Shoes");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoesImages_Shoes_ShoesId",
                table: "ShoesImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Colors_ColorId",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Shoes_ShoesId",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Sizes_SizeId",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Provider");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_ColorId",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_ShoesId",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_SizeId",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_ShoesImages_ShoesId",
                table: "ShoesImages");

            migrationBuilder.DropIndex(
                name: "IX_Shoes_GenderId",
                table: "Shoes");

            migrationBuilder.DropIndex(
                name: "IX_Shoes_ShoesBrandId",
                table: "Shoes");

            migrationBuilder.DropIndex(
                name: "IX_Shoes_ShoesTypeId",
                table: "Shoes");

            migrationBuilder.DropIndex(
                name: "IX_SaleProducts_SaleId",
                table: "SaleProducts");

            migrationBuilder.DropIndex(
                name: "IX_SaleProducts_StockId",
                table: "SaleProducts");

            migrationBuilder.DropIndex(
                name: "IX_Payments_OrderId1",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Imports_ProviderId",
                table: "Imports");

            migrationBuilder.DropIndex(
                name: "IX_ImportDetails_ImportId",
                table: "ImportDetails");

            migrationBuilder.DropIndex(
                name: "IX_ImportDetails_StockId",
                table: "ImportDetails");

            migrationBuilder.DropIndex(
                name: "IX_CustomerReviews_ShoesId",
                table: "CustomerReviews");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_StockId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_Addresss_CustomerId",
                table: "Addresss");

            migrationBuilder.DropColumn(
                name: "ShoesBrandId",
                table: "Shoes");

            migrationBuilder.DropColumn(
                name: "ShoesTypeId",
                table: "Shoes");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "Payments");
        }
    }
}
