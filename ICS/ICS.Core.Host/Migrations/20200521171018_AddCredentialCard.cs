using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICS.Core.Host.Migrations
{
    public partial class AddCredentialCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RFID",
                schema: "Core",
                table: "Clients");

            migrationBuilder.CreateTable(
                name: "CredentialCards",
                schema: "Core",
                columns: table => new
                {
                    CardNumber = table.Column<string>(maxLength: 10, nullable: false),
                    Rfid = table.Column<string>(maxLength: 25, nullable: true),
                    ClientUuid = table.Column<Guid>(nullable: false),
                    Condition = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialCards", x => x.CardNumber);
                    table.ForeignKey(
                        name: "FK_CredentialCards_Clients_ClientUuid",
                        column: x => x.ClientUuid,
                        principalSchema: "Core",
                        principalTable: "Clients",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CredentialCards_ClientUuid",
                schema: "Core",
                table: "CredentialCards",
                column: "ClientUuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CredentialCards_Rfid",
                schema: "Core",
                table: "CredentialCards",
                column: "Rfid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CredentialCards",
                schema: "Core");

            migrationBuilder.AddColumn<string>(
                name: "RFID",
                schema: "Core",
                table: "Clients",
                type: "text",
                nullable: true);
        }
    }
}
