using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ICS.Core.Host.Migrations
{
    public partial class AddSessionParking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceGroupId",
                table: "ParkingPlaces",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SessionParking",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceGroupId = table.Column<int>(nullable: true),
                    ParkingPlaceId = table.Column<int>(nullable: true),
                    ReturnDate = table.Column<DateTime>(nullable: false),
                    TimeOfReceipt = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionParking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionParking_ParkingPlaces_ParkingPlaceId",
                        column: x => x.ParkingPlaceId,
                        principalTable: "ParkingPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SessionParking_ServiceGroups_ServiceGroupId",
                        column: x => x.ServiceGroupId,
                        principalTable: "ServiceGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingPlaces_ServiceGroupId",
                table: "ParkingPlaces",
                column: "ServiceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionParking_ParkingPlaceId",
                table: "SessionParking",
                column: "ParkingPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionParking_ServiceGroupId",
                table: "SessionParking",
                column: "ServiceGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingPlaces_ServiceGroups_ServiceGroupId",
                table: "ParkingPlaces",
                column: "ServiceGroupId",
                principalTable: "ServiceGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingPlaces_ServiceGroups_ServiceGroupId",
                table: "ParkingPlaces");

            migrationBuilder.DropTable(
                name: "SessionParking");

            migrationBuilder.DropIndex(
                name: "IX_ParkingPlaces_ServiceGroupId",
                table: "ParkingPlaces");

            migrationBuilder.DropColumn(
                name: "ServiceGroupId",
                table: "ParkingPlaces");
        }
    }
}
