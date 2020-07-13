using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICS.Core.Host.Migrations
{
    public partial class ChangeSessionParkingModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnDate",
                table: "SessionParkings");

            migrationBuilder.DropColumn(
                name: "TimeOfReceipt",
                table: "SessionParkings");

            migrationBuilder.AddColumn<int>(
                name: "KeepAliveEndSessionId",
                table: "SessionParkings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KeepAliveOpenSessionId",
                table: "SessionParkings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessionParkings_KeepAliveEndSessionId",
                table: "SessionParkings",
                column: "KeepAliveEndSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionParkings_KeepAliveOpenSessionId",
                table: "SessionParkings",
                column: "KeepAliveOpenSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionParkings_ParkingPlaceKeepAlives_KeepAliveEndSessionId",
                table: "SessionParkings",
                column: "KeepAliveEndSessionId",
                principalTable: "ParkingPlaceKeepAlives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionParkings_ParkingPlaceKeepAlives_KeepAliveOpenSession~",
                table: "SessionParkings",
                column: "KeepAliveOpenSessionId",
                principalTable: "ParkingPlaceKeepAlives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionParkings_ParkingPlaceKeepAlives_KeepAliveEndSessionId",
                table: "SessionParkings");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionParkings_ParkingPlaceKeepAlives_KeepAliveOpenSession~",
                table: "SessionParkings");

            migrationBuilder.DropIndex(
                name: "IX_SessionParkings_KeepAliveEndSessionId",
                table: "SessionParkings");

            migrationBuilder.DropIndex(
                name: "IX_SessionParkings_KeepAliveOpenSessionId",
                table: "SessionParkings");

            migrationBuilder.DropColumn(
                name: "KeepAliveEndSessionId",
                table: "SessionParkings");

            migrationBuilder.DropColumn(
                name: "KeepAliveOpenSessionId",
                table: "SessionParkings");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDate",
                table: "SessionParkings",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeOfReceipt",
                table: "SessionParkings",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
