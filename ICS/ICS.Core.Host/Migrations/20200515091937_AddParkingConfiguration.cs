using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ICS.Core.Host.Migrations
{
    public partial class AddParkingConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParkingConfigurations",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ReservationAllowed = table.Column<int>(nullable: false),
                    MaxNumberPlaces = table.Column<int>(nullable: false),
                    MaxNumberDay = table.Column<int>(nullable: false),
                    ParkingId = table.Column<int>(nullable: true),
                    ModifyingId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkingConfigurations_Supervisors_ModifyingId",
                        column: x => x.ModifyingId,
                        principalSchema: "Core",
                        principalTable: "Supervisors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParkingConfigurations_Parkings_ParkingId",
                        column: x => x.ParkingId,
                        principalSchema: "Core",
                        principalTable: "Parkings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingConfigurations_ModifyingId",
                schema: "Core",
                table: "ParkingConfigurations",
                column: "ModifyingId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingConfigurations_ParkingId",
                schema: "Core",
                table: "ParkingConfigurations",
                column: "ParkingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkingConfigurations",
                schema: "Core");
        }
    }
}
