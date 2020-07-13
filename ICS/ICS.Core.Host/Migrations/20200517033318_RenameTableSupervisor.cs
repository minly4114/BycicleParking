using Microsoft.EntityFrameworkCore.Migrations;

namespace ICS.Core.Host.Migrations
{
    public partial class RenameTableSupervisor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Post",
                schema: "Core",
                table: "Supervisors");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                schema: "Core",
                table: "Supervisors",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                schema: "Core",
                table: "Supervisors");

            migrationBuilder.AddColumn<string>(
                name: "Post",
                schema: "Core",
                table: "Supervisors",
                type: "text",
                nullable: true);
        }
    }
}
