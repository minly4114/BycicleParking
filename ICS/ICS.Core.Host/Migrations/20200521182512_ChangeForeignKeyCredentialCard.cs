using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICS.Core.Host.Migrations
{
    public partial class ChangeForeignKeyCredentialCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CredentialCards_Clients_ClientUuid",
                schema: "Core",
                table: "CredentialCards");

            migrationBuilder.DropIndex(
                name: "IX_CredentialCards_ClientUuid",
                schema: "Core",
                table: "CredentialCards");

            migrationBuilder.DropColumn(
                name: "ClientUuid",
                schema: "Core",
                table: "CredentialCards");

            migrationBuilder.AddColumn<string>(
                name: "CredentialCardNumber",
                schema: "Core",
                table: "Clients",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CredentialCardNumber",
                schema: "Core",
                table: "Clients",
                column: "CredentialCardNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_CredentialCards_CredentialCardNumber",
                schema: "Core",
                table: "Clients",
                column: "CredentialCardNumber",
                principalSchema: "Core",
                principalTable: "CredentialCards",
                principalColumn: "CardNumber",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_CredentialCards_CredentialCardNumber",
                schema: "Core",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_CredentialCardNumber",
                schema: "Core",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CredentialCardNumber",
                schema: "Core",
                table: "Clients");

            migrationBuilder.AddColumn<Guid>(
                name: "ClientUuid",
                schema: "Core",
                table: "CredentialCards",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CredentialCards_ClientUuid",
                schema: "Core",
                table: "CredentialCards",
                column: "ClientUuid",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CredentialCards_Clients_ClientUuid",
                schema: "Core",
                table: "CredentialCards",
                column: "ClientUuid",
                principalSchema: "Core",
                principalTable: "Clients",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
