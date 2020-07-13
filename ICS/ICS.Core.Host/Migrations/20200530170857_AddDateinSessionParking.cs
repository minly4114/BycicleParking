using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICS.Core.Host.Migrations
{
    public partial class AddDateinSessionParking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndParking",
                schema: "Core",
                table: "SessionParkings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartParking",
                schema: "Core",
                table: "SessionParkings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndParking",
                schema: "Core",
                table: "SessionParkings");

            migrationBuilder.DropColumn(
                name: "StartParking",
                schema: "Core",
                table: "SessionParkings");
        }
    }
}
