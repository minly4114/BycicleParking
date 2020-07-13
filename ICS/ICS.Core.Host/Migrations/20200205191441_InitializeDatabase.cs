using System;
using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ICS.Core.Host.Migrations
{
    public partial class InitializeDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Supervisors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Uuid = table.Column<Guid>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    PastName = table.Column<string>(nullable: true),
                    Post = table.Column<string>(nullable: true),
                    IsConfirmed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supervisors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clusters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Uuid = table.Column<Guid>(nullable: false),
                    IPAddress = table.Column<IPAddress>(nullable: true),
                    IsConfirmed = table.Column<bool>(nullable: false),
                    SupervisorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clusters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clusters_Supervisors_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "Supervisors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClusterKeepAlives",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    ClusterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClusterKeepAlives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClusterKeepAlives_Clusters_ClusterId",
                        column: x => x.ClusterId,
                        principalTable: "Clusters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClusterTokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Value = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    ExpiredAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClusterTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClusterTokens_Clusters_Id",
                        column: x => x.Id,
                        principalTable: "Clusters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parkings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Uuid = table.Column<Guid>(nullable: false),
                    ClusterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parkings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parkings_Clusters_ClusterId",
                        column: x => x.ClusterId,
                        principalTable: "Clusters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParkingKeepAlives",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    ParkingCondition = table.Column<int>(nullable: false),
                    ParkingId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingKeepAlives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkingKeepAlives_Parkings_ParkingId",
                        column: x => x.ParkingId,
                        principalTable: "Parkings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParkingPlaces",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Uuid = table.Column<Guid>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    Serial = table.Column<int>(nullable: false),
                    ParkingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingPlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkingPlaces_Parkings_ParkingId",
                        column: x => x.ParkingId,
                        principalTable: "Parkings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParkingPlaceKeepAlives",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    ParkingCondition = table.Column<int>(nullable: false),
                    ParkingPlaceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingPlaceKeepAlives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkingPlaceKeepAlives_ParkingPlaces_ParkingPlaceId",
                        column: x => x.ParkingPlaceId,
                        principalTable: "ParkingPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClusterKeepAlives_ClusterId",
                table: "ClusterKeepAlives",
                column: "ClusterId");

            migrationBuilder.CreateIndex(
                name: "IX_Clusters_SupervisorId",
                table: "Clusters",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Clusters_Uuid",
                table: "Clusters",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClusterTokens_Value",
                table: "ClusterTokens",
                column: "Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingKeepAlives_ParkingId",
                table: "ParkingKeepAlives",
                column: "ParkingId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingPlaceKeepAlives_ParkingPlaceId",
                table: "ParkingPlaceKeepAlives",
                column: "ParkingPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingPlaces_ParkingId_Level_Serial",
                table: "ParkingPlaces",
                columns: new[] { "ParkingId", "Level", "Serial" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parkings_ClusterId",
                table: "Parkings",
                column: "ClusterId");

            migrationBuilder.CreateIndex(
                name: "IX_Parkings_Uuid",
                table: "Parkings",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Supervisors_Uuid",
                table: "Supervisors",
                column: "Uuid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClusterKeepAlives");

            migrationBuilder.DropTable(
                name: "ClusterTokens");

            migrationBuilder.DropTable(
                name: "ParkingKeepAlives");

            migrationBuilder.DropTable(
                name: "ParkingPlaceKeepAlives");

            migrationBuilder.DropTable(
                name: "ParkingPlaces");

            migrationBuilder.DropTable(
                name: "Parkings");

            migrationBuilder.DropTable(
                name: "Clusters");

            migrationBuilder.DropTable(
                name: "Supervisors");
        }
    }
}
