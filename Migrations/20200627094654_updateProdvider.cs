using Microsoft.EntityFrameworkCore.Migrations;

namespace shop_giay_server.Migrations
{
    public partial class updateProdvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Providers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Providers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Providers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TIN",
                table: "Providers",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "TIN",
                table: "Providers");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
