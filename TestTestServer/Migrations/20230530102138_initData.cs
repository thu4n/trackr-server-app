using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestTestServer.Migrations
{
    /// <inheritdoc />
    public partial class initData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdPassword = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.AdID);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CusName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CusAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CusPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CusBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CusDateRegister = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CusAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CusPassword = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CusID);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryMan",
                columns: table => new
                {
                    ManID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManPassword = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryMan", x => x.ManID);
                });

            migrationBuilder.CreateTable(
                name: "Parcel",
                columns: table => new
                {
                    ParID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Realtime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: false),
                    CusID = table.Column<int>(type: "int", nullable: false),
                    ManID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcel", x => x.ParID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "DeliveryMan");

            migrationBuilder.DropTable(
                name: "Parcel");
        }
    }
}
