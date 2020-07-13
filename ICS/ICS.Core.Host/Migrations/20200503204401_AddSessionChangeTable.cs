using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ICS.Core.Host.Migrations
{
    public partial class AddSessionChangeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.EnsureSchema(
                name: "Core");

            migrationBuilder.RenameTable(
                name: "Supervisors",
                newName: "Supervisors",
                newSchema: "Core");

            migrationBuilder.RenameTable(
                name: "SessionParkings",
                newName: "SessionParkings",
                newSchema: "Core");

            migrationBuilder.RenameTable(
                name: "ServiceGroups",
                newName: "ServiceGroups",
                newSchema: "Core");

            migrationBuilder.RenameTable(
                name: "Parkings",
                newName: "Parkings",
                newSchema: "Core");

            migrationBuilder.RenameTable(
                name: "ParkingPlaces",
                newName: "ParkingPlaces",
                newSchema: "Core");

            migrationBuilder.RenameTable(
                name: "ParkingPlaceKeepAlives",
                newName: "ParkingPlaceKeepAlives",
                newSchema: "Core");

            migrationBuilder.RenameTable(
                name: "ParkingKeepAlives",
                newName: "ParkingKeepAlives",
                newSchema: "Core");

            migrationBuilder.RenameTable(
                name: "ClusterTokens",
                newName: "ClusterTokens",
                newSchema: "Core");

            migrationBuilder.RenameTable(
                name: "Clusters",
                newName: "Clusters",
                newSchema: "Core");

            migrationBuilder.RenameTable(
                name: "ClusterKeepAlives",
                newName: "ClusterKeepAlives",
                newSchema: "Core");

            migrationBuilder.RenameTable(
                name: "ClientServiceGroup",
                newName: "ClientServiceGroup",
                newSchema: "Core");

            migrationBuilder.RenameTable(
                name: "Clients",
                newName: "Clients",
                newSchema: "Core");

            migrationBuilder.CreateTable(
                name: "SessionChange",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    SessionCondition = table.Column<int>(nullable: false),
                    SessionParkingId = table.Column<int>(nullable: true),
                    ParkingPlaceKeepAliveId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionChange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionChange_ParkingPlaceKeepAlives_ParkingPlaceKeepAliveId",
                        column: x => x.ParkingPlaceKeepAliveId,
                        principalSchema: "Core",
                        principalTable: "ParkingPlaceKeepAlives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SessionChange_SessionParkings_SessionParkingId",
                        column: x => x.SessionParkingId,
                        principalSchema: "Core",
                        principalTable: "SessionParkings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionChange_ParkingPlaceKeepAliveId",
                schema: "Core",
                table: "SessionChange",
                column: "ParkingPlaceKeepAliveId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionChange_SessionParkingId",
                schema: "Core",
                table: "SessionChange",
                column: "SessionParkingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionChange",
                schema: "Core");

            migrationBuilder.RenameTable(
                name: "Supervisors",
                schema: "Core",
                newName: "Supervisors");

            migrationBuilder.RenameTable(
                name: "SessionParkings",
                schema: "Core",
                newName: "SessionParkings");

            migrationBuilder.RenameTable(
                name: "ServiceGroups",
                schema: "Core",
                newName: "ServiceGroups");

            migrationBuilder.RenameTable(
                name: "Parkings",
                schema: "Core",
                newName: "Parkings");

            migrationBuilder.RenameTable(
                name: "ParkingPlaces",
                schema: "Core",
                newName: "ParkingPlaces");

            migrationBuilder.RenameTable(
                name: "ParkingPlaceKeepAlives",
                schema: "Core",
                newName: "ParkingPlaceKeepAlives");

            migrationBuilder.RenameTable(
                name: "ParkingKeepAlives",
                schema: "Core",
                newName: "ParkingKeepAlives");

            migrationBuilder.RenameTable(
                name: "ClusterTokens",
                schema: "Core",
                newName: "ClusterTokens");

            migrationBuilder.RenameTable(
                name: "Clusters",
                schema: "Core",
                newName: "Clusters");

            migrationBuilder.RenameTable(
                name: "ClusterKeepAlives",
                schema: "Core",
                newName: "ClusterKeepAlives");

            migrationBuilder.RenameTable(
                name: "ClientServiceGroup",
                schema: "Core",
                newName: "ClientServiceGroup");

            migrationBuilder.RenameTable(
                name: "Clients",
                schema: "Core",
                newName: "Clients");

            migrationBuilder.AddColumn<int>(
                name: "KeepAliveEndSessionId",
                table: "SessionParkings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KeepAliveOpenSessionId",
                table: "SessionParkings",
                type: "integer",
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
    }
}
