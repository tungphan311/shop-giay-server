using Microsoft.EntityFrameworkCore.Migrations;

namespace shop_giay_server.Migrations
{
    public partial class upatemigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresss_Customers_CustomerId",
                table: "Addresss");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Carts_CartId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CartId",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresss",
                table: "Addresss");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Addresss",
                newName: "Addresses");

            migrationBuilder.RenameIndex(
                name: "IX_Addresss_CustomerId",
                table: "Addresses",
                newName: "IX_Addresses_CustomerId");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Carts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CustomerId",
                table: "Carts",
                column: "CustomerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Customers_CustomerId",
                table: "Addresses",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Customers_CustomerId",
                table: "Carts",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Customers_CustomerId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Customers_CustomerId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_CustomerId",
                table: "Carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Carts");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "Addresss");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_CustomerId",
                table: "Addresss",
                newName: "IX_Addresss_CustomerId");

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresss",
                table: "Addresss",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CartId",
                table: "Customers",
                column: "CartId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresss_Customers_CustomerId",
                table: "Addresss",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Carts_CartId",
                table: "Customers",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
