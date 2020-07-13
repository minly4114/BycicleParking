using Microsoft.EntityFrameworkCore.Migrations;

namespace ICS.Core.Host.Migrations
{
    public partial class ChangeMaxNumberInCredentialCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Rfid",
                schema: "Core",
                table: "CredentialCards",
                maxLength: 34,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CardNumber",
                schema: "Core",
                table: "CredentialCards",
                maxLength: 18,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Rfid",
                schema: "Core",
                table: "CredentialCards",
                type: "character varying(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 34,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CardNumber",
                schema: "Core",
                table: "CredentialCards",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 18);
        }
    }
}
