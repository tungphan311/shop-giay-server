using Microsoft.EntityFrameworkCore.Migrations;

namespace shop_giay_server.Migrations
{
    public partial class updateModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SaleId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SaleId",
                table: "Orders",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Sales_SaleId",
                table: "Orders",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Sales_SaleId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_SaleId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SaleId",
                table: "Orders");
        }
    }
}
