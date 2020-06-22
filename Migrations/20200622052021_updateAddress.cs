using Microsoft.EntityFrameworkCore.Migrations;

namespace shop_giay_server.Migrations
{
    public partial class updateAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecipientName",
                table: "Addresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipientPhoneNumber",
                table: "Addresses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientName",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "RecipientPhoneNumber",
                table: "Addresses");
        }
    }
}
