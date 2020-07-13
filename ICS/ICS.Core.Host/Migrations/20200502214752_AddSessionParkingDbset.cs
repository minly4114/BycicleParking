using Microsoft.EntityFrameworkCore.Migrations;

namespace ICS.Core.Host.Migrations
{
    public partial class AddSessionParkingDbset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionParking_ParkingPlaces_ParkingPlaceId",
                table: "SessionParking");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionParking_ServiceGroups_ServiceGroupId",
                table: "SessionParking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionParking",
                table: "SessionParking");

            migrationBuilder.RenameTable(
                name: "SessionParking",
                newName: "SessionParkings");

            migrationBuilder.RenameIndex(
                name: "IX_SessionParking_ServiceGroupId",
                table: "SessionParkings",
                newName: "IX_SessionParkings_ServiceGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_SessionParking_ParkingPlaceId",
                table: "SessionParkings",
                newName: "IX_SessionParkings_ParkingPlaceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionParkings",
                table: "SessionParkings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionParkings_ParkingPlaces_ParkingPlaceId",
                table: "SessionParkings",
                column: "ParkingPlaceId",
                principalTable: "ParkingPlaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionParkings_ServiceGroups_ServiceGroupId",
                table: "SessionParkings",
                column: "ServiceGroupId",
                principalTable: "ServiceGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionParkings_ParkingPlaces_ParkingPlaceId",
                table: "SessionParkings");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionParkings_ServiceGroups_ServiceGroupId",
                table: "SessionParkings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionParkings",
                table: "SessionParkings");

            migrationBuilder.RenameTable(
                name: "SessionParkings",
                newName: "SessionParking");

            migrationBuilder.RenameIndex(
                name: "IX_SessionParkings_ServiceGroupId",
                table: "SessionParking",
                newName: "IX_SessionParking_ServiceGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_SessionParkings_ParkingPlaceId",
                table: "SessionParking",
                newName: "IX_SessionParking_ParkingPlaceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionParking",
                table: "SessionParking",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionParking_ParkingPlaces_ParkingPlaceId",
                table: "SessionParking",
                column: "ParkingPlaceId",
                principalTable: "ParkingPlaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionParking_ServiceGroups_ServiceGroupId",
                table: "SessionParking",
                column: "ServiceGroupId",
                principalTable: "ServiceGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
