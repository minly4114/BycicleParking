using Microsoft.EntityFrameworkCore.Migrations;

namespace ICS.Core.Host.Migrations
{
    public partial class AddNameInParking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "LocationLat",
                schema: "Core",
                table: "Parkings",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "LocationLng",
                schema: "Core",
                table: "Parkings",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Core",
                table: "Parkings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationLat",
                schema: "Core",
                table: "Parkings");

            migrationBuilder.DropColumn(
                name: "LocationLng",
                schema: "Core",
                table: "Parkings");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Core",
                table: "Parkings");
        }
    }
}
