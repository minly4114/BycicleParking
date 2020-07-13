using Microsoft.EntityFrameworkCore.Migrations;

namespace ICS.Core.Host.Migrations
{
    public partial class ChangeMaxNumberRfidInCredentialCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Rfid",
                schema: "Core",
                table: "CredentialCards",
                maxLength: 36,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(34)",
                oldMaxLength: 34,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Rfid",
                schema: "Core",
                table: "CredentialCards",
                type: "character varying(34)",
                maxLength: 34,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 36,
                oldNullable: true);
        }
    }
}
