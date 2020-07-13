using Microsoft.EntityFrameworkCore.Migrations;

namespace ICS.Core.Host.Migrations
{
    public partial class AddIndexInSessionChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionChange_SessionParkings_SessionParkingId",
                schema: "Core",
                table: "SessionChange");

            migrationBuilder.AlterColumn<int>(
                name: "SessionParkingId",
                schema: "Core",
                table: "SessionChange",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessionChange_SessionCondition_SessionParkingId",
                schema: "Core",
                table: "SessionChange",
                columns: new[] { "SessionCondition", "SessionParkingId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionChange_SessionParkings_SessionParkingId",
                schema: "Core",
                table: "SessionChange",
                column: "SessionParkingId",
                principalSchema: "Core",
                principalTable: "SessionParkings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionChange_SessionParkings_SessionParkingId",
                schema: "Core",
                table: "SessionChange");

            migrationBuilder.DropIndex(
                name: "IX_SessionChange_SessionCondition_SessionParkingId",
                schema: "Core",
                table: "SessionChange");

            migrationBuilder.AlterColumn<int>(
                name: "SessionParkingId",
                schema: "Core",
                table: "SessionChange",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_SessionChange_SessionParkings_SessionParkingId",
                schema: "Core",
                table: "SessionChange",
                column: "SessionParkingId",
                principalSchema: "Core",
                principalTable: "SessionParkings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
