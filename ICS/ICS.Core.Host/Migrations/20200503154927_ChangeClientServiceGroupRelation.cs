using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ICS.Core.Host.Migrations
{
    public partial class ChangeClientServiceGroupRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_ServiceGroups_ServiceGroupId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceGroups_Clients_ClientId",
                table: "ServiceGroups");

            migrationBuilder.DropIndex(
                name: "IX_ServiceGroups_ClientId",
                table: "ServiceGroups");

            migrationBuilder.DropIndex(
                name: "IX_Clients_ServiceGroupId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "ServiceGroups");

            migrationBuilder.DropColumn(
                name: "ServiceGroupId",
                table: "Clients");

            migrationBuilder.CreateTable(
                name: "ClientServiceGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<int>(nullable: false),
                    ServiceGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientServiceGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientServiceGroup_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientServiceGroup_ServiceGroups_ServiceGroupId",
                        column: x => x.ServiceGroupId,
                        principalTable: "ServiceGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientServiceGroup_ServiceGroupId",
                table: "ClientServiceGroup",
                column: "ServiceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientServiceGroup_ClientId_ServiceGroupId",
                table: "ClientServiceGroup",
                columns: new[] { "ClientId", "ServiceGroupId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientServiceGroup");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "ServiceGroups",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceGroupId",
                table: "Clients",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceGroups_ClientId",
                table: "ServiceGroups",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ServiceGroupId",
                table: "Clients",
                column: "ServiceGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_ServiceGroups_ServiceGroupId",
                table: "Clients",
                column: "ServiceGroupId",
                principalTable: "ServiceGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceGroups_Clients_ClientId",
                table: "ServiceGroups",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
