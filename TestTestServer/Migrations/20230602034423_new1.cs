using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestTestServer.Migrations
{
    /// <inheritdoc />
    public partial class new1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManImage",
                table: "DeliveryMan",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CusImage",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdImage",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManImage",
                table: "DeliveryMan");

            migrationBuilder.DropColumn(
                name: "CusImage",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "AdImage",
                table: "Admins");
        }
    }
}
