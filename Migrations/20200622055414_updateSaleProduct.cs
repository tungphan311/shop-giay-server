using Microsoft.EntityFrameworkCore.Migrations;

namespace shop_giay_server.Migrations
{
    public partial class updateSaleProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleProducts_Stocks_StockId",
                table: "SaleProducts");

            migrationBuilder.DropIndex(
                name: "IX_SaleProducts_StockId",
                table: "SaleProducts");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "SaleProducts");

            migrationBuilder.AddColumn<int>(
                name: "ShoesId",
                table: "SaleProducts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SaleProducts_ShoesId",
                table: "SaleProducts",
                column: "ShoesId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleProducts_Shoes_ShoesId",
                table: "SaleProducts",
                column: "ShoesId",
                principalTable: "Shoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleProducts_Shoes_ShoesId",
                table: "SaleProducts");

            migrationBuilder.DropIndex(
                name: "IX_SaleProducts_ShoesId",
                table: "SaleProducts");

            migrationBuilder.DropColumn(
                name: "ShoesId",
                table: "SaleProducts");

            migrationBuilder.AddColumn<int>(
                name: "StockId",
                table: "SaleProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SaleProducts_StockId",
                table: "SaleProducts",
                column: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleProducts_Stocks_StockId",
                table: "SaleProducts",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
