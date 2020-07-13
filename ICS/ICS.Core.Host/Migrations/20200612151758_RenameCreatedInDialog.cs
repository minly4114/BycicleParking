using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICS.Core.Host.Migrations
{
    public partial class RenameCreatedInDialog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clusters_Supervisors_SupervisorUuid",
                schema: "Core",
                table: "Clusters");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingConfigurations_Supervisors_ModifyingUuid",
                schema: "Core",
                table: "ParkingConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Supervisors",
                schema: "Core",
                table: "Supervisors");

            migrationBuilder.DropColumn(
                name: "CraetedAt",
                schema: "Core",
                table: "Dialogs");

            migrationBuilder.RenameTable(
                name: "Supervisors",
                schema: "Core",
                newName: "Workers",
                newSchema: "Core");

            migrationBuilder.RenameIndex(
                name: "IX_Supervisors_Uuid",
                schema: "Core",
                table: "Workers",
                newName: "IX_Workers_Uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "Core",
                table: "Dialogs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Workers",
                schema: "Core",
                table: "Workers",
                column: "Uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Clusters_Workers_SupervisorUuid",
                schema: "Core",
                table: "Clusters",
                column: "SupervisorUuid",
                principalSchema: "Core",
                principalTable: "Workers",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingConfigurations_Workers_ModifyingUuid",
                schema: "Core",
                table: "ParkingConfigurations",
                column: "ModifyingUuid",
                principalSchema: "Core",
                principalTable: "Workers",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clusters_Workers_SupervisorUuid",
                schema: "Core",
                table: "Clusters");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingConfigurations_Workers_ModifyingUuid",
                schema: "Core",
                table: "ParkingConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Workers",
                schema: "Core",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "Core",
                table: "Dialogs");

            migrationBuilder.RenameTable(
                name: "Workers",
                schema: "Core",
                newName: "Supervisors",
                newSchema: "Core");

            migrationBuilder.RenameIndex(
                name: "IX_Workers_Uuid",
                schema: "Core",
                table: "Supervisors",
                newName: "IX_Supervisors_Uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CraetedAt",
                schema: "Core",
                table: "Dialogs",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Supervisors",
                schema: "Core",
                table: "Supervisors",
                column: "Uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Clusters_Supervisors_SupervisorUuid",
                schema: "Core",
                table: "Clusters",
                column: "SupervisorUuid",
                principalSchema: "Core",
                principalTable: "Supervisors",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingConfigurations_Supervisors_ModifyingUuid",
                schema: "Core",
                table: "ParkingConfigurations",
                column: "ModifyingUuid",
                principalSchema: "Core",
                principalTable: "Supervisors",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
