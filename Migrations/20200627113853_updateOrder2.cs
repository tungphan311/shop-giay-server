using Microsoft.EntityFrameworkCore.Migrations;

namespace shop_giay_server.Migrations
{
    public partial class updateOrder2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecipientName",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipientPhoneNumber",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RecipientPhoneNumber",
                table: "Orders");
        }
    }
}
