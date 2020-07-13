using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ICS.Core.Host.Migrations
{
    public partial class ChangeParkingPlaceKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingPlaceKeepAlives_ParkingPlaces_ParkingPlaceId",
                schema: "Core",
                table: "ParkingPlaceKeepAlives");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionParkings_ParkingPlaces_ParkingPlaceId",
                schema: "Core",
                table: "SessionParkings");

            migrationBuilder.DropIndex(
                name: "IX_SessionParkings_ParkingPlaceId",
                schema: "Core",
                table: "SessionParkings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkingPlaces",
                schema: "Core",
                table: "ParkingPlaces");

            migrationBuilder.DropIndex(
                name: "IX_ParkingPlaceKeepAlives_ParkingPlaceId",
                schema: "Core",
                table: "ParkingPlaceKeepAlives");

            migrationBuilder.DropColumn(
                name: "ParkingPlaceId",
                schema: "Core",
                table: "SessionParkings");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Core",
                table: "ParkingPlaces");

            migrationBuilder.DropColumn(
                name: "ParkingPlaceId",
                schema: "Core",
                table: "ParkingPlaceKeepAlives");

            migrationBuilder.AddColumn<Guid>(
                name: "ParkingPlaceUuid",
                schema: "Core",
                table: "SessionParkings",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParkingPlaceUuid",
                schema: "Core",
                table: "ParkingPlaceKeepAlives",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkingPlaces",
                schema: "Core",
                table: "ParkingPlaces",
                column: "Uuid");

            migrationBuilder.CreateIndex(
                name: "IX_SessionParkings_ParkingPlaceUuid",
                schema: "Core",
                table: "SessionParkings",
                column: "ParkingPlaceUuid");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingPlaceKeepAlives_ParkingPlaceUuid",
                schema: "Core",
                table: "ParkingPlaceKeepAlives",
                column: "ParkingPlaceUuid");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingPlaceKeepAlives_ParkingPlaces_ParkingPlaceUuid",
                schema: "Core",
                table: "ParkingPlaceKeepAlives",
                column: "ParkingPlaceUuid",
                principalSchema: "Core",
                principalTable: "ParkingPlaces",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionParkings_ParkingPlaces_ParkingPlaceUuid",
                schema: "Core",
                table: "SessionParkings",
                column: "ParkingPlaceUuid",
                principalSchema: "Core",
                principalTable: "ParkingPlaces",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingPlaceKeepAlives_ParkingPlaces_ParkingPlaceUuid",
                schema: "Core",
                table: "ParkingPlaceKeepAlives");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionParkings_ParkingPlaces_ParkingPlaceUuid",
                schema: "Core",
                table: "SessionParkings");

            migrationBuilder.DropIndex(
                name: "IX_SessionParkings_ParkingPlaceUuid",
                schema: "Core",
                table: "SessionParkings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkingPlaces",
                schema: "Core",
                table: "ParkingPlaces");

            migrationBuilder.DropIndex(
                name: "IX_ParkingPlaceKeepAlives_ParkingPlaceUuid",
                schema: "Core",
                table: "ParkingPlaceKeepAlives");

            migrationBuilder.DropColumn(
                name: "ParkingPlaceUuid",
                schema: "Core",
                table: "SessionParkings");

            migrationBuilder.DropColumn(
                name: "ParkingPlaceUuid",
                schema: "Core",
                table: "ParkingPlaceKeepAlives");

            migrationBuilder.AddColumn<int>(
                name: "ParkingPlaceId",
                schema: "Core",
                table: "SessionParkings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Core",
                table: "ParkingPlaces",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "ParkingPlaceId",
                schema: "Core",
                table: "ParkingPlaceKeepAlives",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkingPlaces",
                schema: "Core",
                table: "ParkingPlaces",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SessionParkings_ParkingPlaceId",
                schema: "Core",
                table: "SessionParkings",
                column: "ParkingPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingPlaceKeepAlives_ParkingPlaceId",
                schema: "Core",
                table: "ParkingPlaceKeepAlives",
                column: "ParkingPlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingPlaceKeepAlives_ParkingPlaces_ParkingPlaceId",
                schema: "Core",
                table: "ParkingPlaceKeepAlives",
                column: "ParkingPlaceId",
                principalSchema: "Core",
                principalTable: "ParkingPlaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionParkings_ParkingPlaces_ParkingPlaceId",
                schema: "Core",
                table: "SessionParkings",
                column: "ParkingPlaceId",
                principalSchema: "Core",
                principalTable: "ParkingPlaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
