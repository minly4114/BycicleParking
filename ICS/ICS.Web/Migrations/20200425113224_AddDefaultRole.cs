using Microsoft.EntityFrameworkCore.Migrations;

namespace ICS.Web.Migrations
{
    public partial class AddDefaultRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Authorization",
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1407f110-186a-42ff-9d5e-76d52b6fcb6c", "5d103a85-5010-4ff0-a6f4-826b78fcf028", "Administrator", null },
                    { "b53feb98-b582-481f-88c0-ab05b261de0d", "6d0525af-0b5c-4585-b958-df56b0cb7397", "Supervizer", null },
                    { "40b553eb-e31f-4fd5-8e9f-b0c2d2c0020d", "217986be-87a7-4a0d-a905-56acaf759701", "Engineer", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Authorization",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1407f110-186a-42ff-9d5e-76d52b6fcb6c");

            migrationBuilder.DeleteData(
                schema: "Authorization",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "40b553eb-e31f-4fd5-8e9f-b0c2d2c0020d");

            migrationBuilder.DeleteData(
                schema: "Authorization",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b53feb98-b582-481f-88c0-ab05b261de0d");
        }
    }
}
