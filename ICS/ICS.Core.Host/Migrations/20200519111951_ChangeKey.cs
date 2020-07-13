using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ICS.Core.Host.Migrations
{
    public partial class ChangeKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientServiceGroup_Clients_ClientId",
                schema: "Core",
                table: "ClientServiceGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientServiceGroup_ServiceGroups_ServiceGroupId",
                schema: "Core",
                table: "ClientServiceGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_ClusterKeepAlives_Clusters_ClusterId",
                schema: "Core",
                table: "ClusterKeepAlives");

            migrationBuilder.DropForeignKey(
                name: "FK_Clusters_Supervisors_SupervisorId",
                schema: "Core",
                table: "Clusters");

            migrationBuilder.DropForeignKey(
                name: "FK_ClusterTokens_Clusters_Id",
                schema: "Core",
                table: "ClusterTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingConfigurations_Supervisors_ModifyingId",
                schema: "Core",
                table: "ParkingConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingConfigurations_Parkings_ParkingId",
                schema: "Core",
                table: "ParkingConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingKeepAlives_Parkings_ParkingId",
                schema: "Core",
                table: "ParkingKeepAlives");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingPlaces_Parkings_ParkingId",
                schema: "Core",
                table: "ParkingPlaces");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingPlaces_ServiceGroups_ServiceGroupId",
                schema: "Core",
                table: "ParkingPlaces");

            migrationBuilder.DropForeignKey(
                name: "FK_Parkings_Clusters_ClusterId",
                schema: "Core",
                table: "Parkings");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionChange_SessionParkings_SessionParkingId",
                schema: "Core",
                table: "SessionChange");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionParkings_ServiceGroups_ServiceGroupId",
                schema: "Core",
                table: "SessionParkings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Supervisors",
                schema: "Core",
                table: "Supervisors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionParkings",
                schema: "Core",
                table: "SessionParkings");

            migrationBuilder.DropIndex(
                name: "IX_SessionParkings_ServiceGroupId",
                schema: "Core",
                table: "SessionParkings");

            migrationBuilder.DropIndex(
                name: "IX_SessionChange_SessionParkingId",
                schema: "Core",
                table: "SessionChange");

            migrationBuilder.DropIndex(
                name: "IX_SessionChange_SessionCondition_SessionParkingId",
                schema: "Core",
                table: "SessionChange");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceGroups",
                schema: "Core",
                table: "ServiceGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Parkings",
                schema: "Core",
                table: "Parkings");

            migrationBuilder.DropIndex(
                name: "IX_Parkings_ClusterId",
                schema: "Core",
                table: "Parkings");

            migrationBuilder.DropIndex(
                name: "IX_ParkingPlaces_ServiceGroupId",
                schema: "Core",
                table: "ParkingPlaces");

            migrationBuilder.DropIndex(
                name: "IX_ParkingPlaces_ParkingId_Level_Serial",
                schema: "Core",
                table: "ParkingPlaces");

            migrationBuilder.DropIndex(
                name: "IX_ParkingKeepAlives_ParkingId",
                schema: "Core",
                table: "ParkingKeepAlives");

            migrationBuilder.DropIndex(
                name: "IX_ParkingConfigurations_ModifyingId",
                schema: "Core",
                table: "ParkingConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_ParkingConfigurations_ParkingId",
                schema: "Core",
                table: "ParkingConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClusterTokens",
                schema: "Core",
                table: "ClusterTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clusters",
                schema: "Core",
                table: "Clusters");

            migrationBuilder.DropIndex(
                name: "IX_Clusters_SupervisorId",
                schema: "Core",
                table: "Clusters");

            migrationBuilder.DropIndex(
                name: "IX_ClusterKeepAlives_ClusterId",
                schema: "Core",
                table: "ClusterKeepAlives");

            migrationBuilder.DropIndex(
                name: "IX_ClientServiceGroup_ServiceGroupId",
                schema: "Core",
                table: "ClientServiceGroup");

            migrationBuilder.DropIndex(
                name: "IX_ClientServiceGroup_ClientId_ServiceGroupId",
                schema: "Core",
                table: "ClientServiceGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clients",
                schema: "Core",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Core",
                table: "Supervisors");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Core",
                table: "SessionParkings");

            migrationBuilder.DropColumn(
                name: "ServiceGroupId",
                schema: "Core",
                table: "SessionParkings");

            migrationBuilder.DropColumn(
                name: "SessionParkingId",
                schema: "Core",
                table: "SessionChange");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Core",
                table: "ServiceGroups");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Core",
                table: "Parkings");

            migrationBuilder.DropColumn(
                name: "ClusterId",
                schema: "Core",
                table: "Parkings");

            migrationBuilder.DropColumn(
                name: "ParkingId",
                schema: "Core",
                table: "ParkingPlaces");

            migrationBuilder.DropColumn(
                name: "ServiceGroupId",
                schema: "Core",
                table: "ParkingPlaces");

            migrationBuilder.DropColumn(
                name: "ParkingId",
                schema: "Core",
                table: "ParkingKeepAlives");

            migrationBuilder.DropColumn(
                name: "ModifyingId",
                schema: "Core",
                table: "ParkingConfigurations");

            migrationBuilder.DropColumn(
                name: "ParkingId",
                schema: "Core",
                table: "ParkingConfigurations");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Core",
                table: "ClusterTokens");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Core",
                table: "Clusters");

            migrationBuilder.DropColumn(
                name: "SupervisorId",
                schema: "Core",
                table: "Clusters");

            migrationBuilder.DropColumn(
                name: "ClusterId",
                schema: "Core",
                table: "ClusterKeepAlives");

            migrationBuilder.DropColumn(
                name: "ClientId",
                schema: "Core",
                table: "ClientServiceGroup");

            migrationBuilder.DropColumn(
                name: "ServiceGroupId",
                schema: "Core",
                table: "ClientServiceGroup");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Core",
                table: "Clients");

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceGroupUuid",
                schema: "Core",
                table: "SessionParkings",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SessionParkingUuid",
                schema: "Core",
                table: "SessionChange",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ClusterUuid",
                schema: "Core",
                table: "Parkings",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParkingUuid",
                schema: "Core",
                table: "ParkingPlaces",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceGroupUuid",
                schema: "Core",
                table: "ParkingPlaces",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParkingUuid",
                schema: "Core",
                table: "ParkingKeepAlives",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifyingUuid",
                schema: "Core",
                table: "ParkingConfigurations",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParkingUuid",
                schema: "Core",
                table: "ParkingConfigurations",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ClusterUuid",
                schema: "Core",
                table: "ClusterTokens",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SupervisorUuid",
                schema: "Core",
                table: "Clusters",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ClusterUuid",
                schema: "Core",
                table: "ClusterKeepAlives",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ClientUuid",
                schema: "Core",
                table: "ClientServiceGroup",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceGroupUuid",
                schema: "Core",
                table: "ClientServiceGroup",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Supervisors",
                schema: "Core",
                table: "Supervisors",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionParkings",
                schema: "Core",
                table: "SessionParkings",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceGroups",
                schema: "Core",
                table: "ServiceGroups",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parkings",
                schema: "Core",
                table: "Parkings",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClusterTokens",
                schema: "Core",
                table: "ClusterTokens",
                column: "ClusterUuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clusters",
                schema: "Core",
                table: "Clusters",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clients",
                schema: "Core",
                table: "Clients",
                column: "Uuid");

            migrationBuilder.CreateIndex(
                name: "IX_SessionParkings_ServiceGroupUuid",
                schema: "Core",
                table: "SessionParkings",
                column: "ServiceGroupUuid");

            migrationBuilder.CreateIndex(
                name: "IX_SessionChange_SessionParkingUuid",
                schema: "Core",
                table: "SessionChange",
                column: "SessionParkingUuid");

            migrationBuilder.CreateIndex(
                name: "IX_SessionChange_SessionCondition_SessionParkingUuid",
                schema: "Core",
                table: "SessionChange",
                columns: new[] { "SessionCondition", "SessionParkingUuid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parkings_ClusterUuid",
                schema: "Core",
                table: "Parkings",
                column: "ClusterUuid");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingPlaces_ServiceGroupUuid",
                schema: "Core",
                table: "ParkingPlaces",
                column: "ServiceGroupUuid");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingPlaces_ParkingUuid_Level_Serial",
                schema: "Core",
                table: "ParkingPlaces",
                columns: new[] { "ParkingUuid", "Level", "Serial" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingKeepAlives_ParkingUuid",
                schema: "Core",
                table: "ParkingKeepAlives",
                column: "ParkingUuid");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingConfigurations_ModifyingUuid",
                schema: "Core",
                table: "ParkingConfigurations",
                column: "ModifyingUuid");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingConfigurations_ParkingUuid",
                schema: "Core",
                table: "ParkingConfigurations",
                column: "ParkingUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Clusters_SupervisorUuid",
                schema: "Core",
                table: "Clusters",
                column: "SupervisorUuid");

            migrationBuilder.CreateIndex(
                name: "IX_ClusterKeepAlives_ClusterUuid",
                schema: "Core",
                table: "ClusterKeepAlives",
                column: "ClusterUuid");

            migrationBuilder.CreateIndex(
                name: "IX_ClientServiceGroup_ServiceGroupUuid",
                schema: "Core",
                table: "ClientServiceGroup",
                column: "ServiceGroupUuid");

            migrationBuilder.CreateIndex(
                name: "IX_ClientServiceGroup_ClientUuid_ServiceGroupUuid",
                schema: "Core",
                table: "ClientServiceGroup",
                columns: new[] { "ClientUuid", "ServiceGroupUuid" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientServiceGroup_Clients_ClientUuid",
                schema: "Core",
                table: "ClientServiceGroup",
                column: "ClientUuid",
                principalSchema: "Core",
                principalTable: "Clients",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientServiceGroup_ServiceGroups_ServiceGroupUuid",
                schema: "Core",
                table: "ClientServiceGroup",
                column: "ServiceGroupUuid",
                principalSchema: "Core",
                principalTable: "ServiceGroups",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClusterKeepAlives_Clusters_ClusterUuid",
                schema: "Core",
                table: "ClusterKeepAlives",
                column: "ClusterUuid",
                principalSchema: "Core",
                principalTable: "Clusters",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_ClusterTokens_Clusters_ClusterUuid",
                schema: "Core",
                table: "ClusterTokens",
                column: "ClusterUuid",
                principalSchema: "Core",
                principalTable: "Clusters",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingConfigurations_Supervisors_ModifyingUuid",
                schema: "Core",
                table: "ParkingConfigurations",
                column: "ModifyingUuid",
                principalSchema: "Core",
                principalTable: "Supervisors",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingConfigurations_Parkings_ParkingUuid",
                schema: "Core",
                table: "ParkingConfigurations",
                column: "ParkingUuid",
                principalSchema: "Core",
                principalTable: "Parkings",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingKeepAlives_Parkings_ParkingUuid",
                schema: "Core",
                table: "ParkingKeepAlives",
                column: "ParkingUuid",
                principalSchema: "Core",
                principalTable: "Parkings",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingPlaces_Parkings_ParkingUuid",
                schema: "Core",
                table: "ParkingPlaces",
                column: "ParkingUuid",
                principalSchema: "Core",
                principalTable: "Parkings",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingPlaces_ServiceGroups_ServiceGroupUuid",
                schema: "Core",
                table: "ParkingPlaces",
                column: "ServiceGroupUuid",
                principalSchema: "Core",
                principalTable: "ServiceGroups",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parkings_Clusters_ClusterUuid",
                schema: "Core",
                table: "Parkings",
                column: "ClusterUuid",
                principalSchema: "Core",
                principalTable: "Clusters",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionChange_SessionParkings_SessionParkingUuid",
                schema: "Core",
                table: "SessionChange",
                column: "SessionParkingUuid",
                principalSchema: "Core",
                principalTable: "SessionParkings",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionParkings_ServiceGroups_ServiceGroupUuid",
                schema: "Core",
                table: "SessionParkings",
                column: "ServiceGroupUuid",
                principalSchema: "Core",
                principalTable: "ServiceGroups",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientServiceGroup_Clients_ClientUuid",
                schema: "Core",
                table: "ClientServiceGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientServiceGroup_ServiceGroups_ServiceGroupUuid",
                schema: "Core",
                table: "ClientServiceGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_ClusterKeepAlives_Clusters_ClusterUuid",
                schema: "Core",
                table: "ClusterKeepAlives");

            migrationBuilder.DropForeignKey(
                name: "FK_Clusters_Supervisors_SupervisorUuid",
                schema: "Core",
                table: "Clusters");

            migrationBuilder.DropForeignKey(
                name: "FK_ClusterTokens_Clusters_ClusterUuid",
                schema: "Core",
                table: "ClusterTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingConfigurations_Supervisors_ModifyingUuid",
                schema: "Core",
                table: "ParkingConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingConfigurations_Parkings_ParkingUuid",
                schema: "Core",
                table: "ParkingConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingKeepAlives_Parkings_ParkingUuid",
                schema: "Core",
                table: "ParkingKeepAlives");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingPlaces_Parkings_ParkingUuid",
                schema: "Core",
                table: "ParkingPlaces");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingPlaces_ServiceGroups_ServiceGroupUuid",
                schema: "Core",
                table: "ParkingPlaces");

            migrationBuilder.DropForeignKey(
                name: "FK_Parkings_Clusters_ClusterUuid",
                schema: "Core",
                table: "Parkings");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionChange_SessionParkings_SessionParkingUuid",
                schema: "Core",
                table: "SessionChange");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionParkings_ServiceGroups_ServiceGroupUuid",
                schema: "Core",
                table: "SessionParkings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Supervisors",
                schema: "Core",
                table: "Supervisors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionParkings",
                schema: "Core",
                table: "SessionParkings");

            migrationBuilder.DropIndex(
                name: "IX_SessionParkings_ServiceGroupUuid",
                schema: "Core",
                table: "SessionParkings");

            migrationBuilder.DropIndex(
                name: "IX_SessionChange_SessionParkingUuid",
                schema: "Core",
                table: "SessionChange");

            migrationBuilder.DropIndex(
                name: "IX_SessionChange_SessionCondition_SessionParkingUuid",
                schema: "Core",
                table: "SessionChange");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceGroups",
                schema: "Core",
                table: "ServiceGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Parkings",
                schema: "Core",
                table: "Parkings");

            migrationBuilder.DropIndex(
                name: "IX_Parkings_ClusterUuid",
                schema: "Core",
                table: "Parkings");

            migrationBuilder.DropIndex(
                name: "IX_ParkingPlaces_ServiceGroupUuid",
                schema: "Core",
                table: "ParkingPlaces");

            migrationBuilder.DropIndex(
                name: "IX_ParkingPlaces_ParkingUuid_Level_Serial",
                schema: "Core",
                table: "ParkingPlaces");

            migrationBuilder.DropIndex(
                name: "IX_ParkingKeepAlives_ParkingUuid",
                schema: "Core",
                table: "ParkingKeepAlives");

            migrationBuilder.DropIndex(
                name: "IX_ParkingConfigurations_ModifyingUuid",
                schema: "Core",
                table: "ParkingConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_ParkingConfigurations_ParkingUuid",
                schema: "Core",
                table: "ParkingConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClusterTokens",
                schema: "Core",
                table: "ClusterTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clusters",
                schema: "Core",
                table: "Clusters");

            migrationBuilder.DropIndex(
                name: "IX_Clusters_SupervisorUuid",
                schema: "Core",
                table: "Clusters");

            migrationBuilder.DropIndex(
                name: "IX_ClusterKeepAlives_ClusterUuid",
                schema: "Core",
                table: "ClusterKeepAlives");

            migrationBuilder.DropIndex(
                name: "IX_ClientServiceGroup_ServiceGroupUuid",
                schema: "Core",
                table: "ClientServiceGroup");

            migrationBuilder.DropIndex(
                name: "IX_ClientServiceGroup_ClientUuid_ServiceGroupUuid",
                schema: "Core",
                table: "ClientServiceGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clients",
                schema: "Core",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ServiceGroupUuid",
                schema: "Core",
                table: "SessionParkings");

            migrationBuilder.DropColumn(
                name: "SessionParkingUuid",
                schema: "Core",
                table: "SessionChange");

            migrationBuilder.DropColumn(
                name: "ClusterUuid",
                schema: "Core",
                table: "Parkings");

            migrationBuilder.DropColumn(
                name: "ParkingUuid",
                schema: "Core",
                table: "ParkingPlaces");

            migrationBuilder.DropColumn(
                name: "ServiceGroupUuid",
                schema: "Core",
                table: "ParkingPlaces");

            migrationBuilder.DropColumn(
                name: "ParkingUuid",
                schema: "Core",
                table: "ParkingKeepAlives");

            migrationBuilder.DropColumn(
                name: "ModifyingUuid",
                schema: "Core",
                table: "ParkingConfigurations");

            migrationBuilder.DropColumn(
                name: "ParkingUuid",
                schema: "Core",
                table: "ParkingConfigurations");

            migrationBuilder.DropColumn(
                name: "ClusterUuid",
                schema: "Core",
                table: "ClusterTokens");

            migrationBuilder.DropColumn(
                name: "SupervisorUuid",
                schema: "Core",
                table: "Clusters");

            migrationBuilder.DropColumn(
                name: "ClusterUuid",
                schema: "Core",
                table: "ClusterKeepAlives");

            migrationBuilder.DropColumn(
                name: "ClientUuid",
                schema: "Core",
                table: "ClientServiceGroup");

            migrationBuilder.DropColumn(
                name: "ServiceGroupUuid",
                schema: "Core",
                table: "ClientServiceGroup");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Core",
                table: "Supervisors",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Core",
                table: "SessionParkings",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "ServiceGroupId",
                schema: "Core",
                table: "SessionParkings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SessionParkingId",
                schema: "Core",
                table: "SessionChange",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Core",
                table: "ServiceGroups",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Core",
                table: "Parkings",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "ClusterId",
                schema: "Core",
                table: "Parkings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParkingId",
                schema: "Core",
                table: "ParkingPlaces",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ServiceGroupId",
                schema: "Core",
                table: "ParkingPlaces",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParkingId",
                schema: "Core",
                table: "ParkingKeepAlives",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifyingId",
                schema: "Core",
                table: "ParkingConfigurations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParkingId",
                schema: "Core",
                table: "ParkingConfigurations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Core",
                table: "ClusterTokens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Core",
                table: "Clusters",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "SupervisorId",
                schema: "Core",
                table: "Clusters",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClusterId",
                schema: "Core",
                table: "ClusterKeepAlives",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                schema: "Core",
                table: "ClientServiceGroup",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ServiceGroupId",
                schema: "Core",
                table: "ClientServiceGroup",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Core",
                table: "Clients",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Supervisors",
                schema: "Core",
                table: "Supervisors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionParkings",
                schema: "Core",
                table: "SessionParkings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceGroups",
                schema: "Core",
                table: "ServiceGroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parkings",
                schema: "Core",
                table: "Parkings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClusterTokens",
                schema: "Core",
                table: "ClusterTokens",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clusters",
                schema: "Core",
                table: "Clusters",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clients",
                schema: "Core",
                table: "Clients",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SessionParkings_ServiceGroupId",
                schema: "Core",
                table: "SessionParkings",
                column: "ServiceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionChange_SessionParkingId",
                schema: "Core",
                table: "SessionChange",
                column: "SessionParkingId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionChange_SessionCondition_SessionParkingId",
                schema: "Core",
                table: "SessionChange",
                columns: new[] { "SessionCondition", "SessionParkingId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parkings_ClusterId",
                schema: "Core",
                table: "Parkings",
                column: "ClusterId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingPlaces_ServiceGroupId",
                schema: "Core",
                table: "ParkingPlaces",
                column: "ServiceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingPlaces_ParkingId_Level_Serial",
                schema: "Core",
                table: "ParkingPlaces",
                columns: new[] { "ParkingId", "Level", "Serial" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingKeepAlives_ParkingId",
                schema: "Core",
                table: "ParkingKeepAlives",
                column: "ParkingId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingConfigurations_ModifyingId",
                schema: "Core",
                table: "ParkingConfigurations",
                column: "ModifyingId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingConfigurations_ParkingId",
                schema: "Core",
                table: "ParkingConfigurations",
                column: "ParkingId");

            migrationBuilder.CreateIndex(
                name: "IX_Clusters_SupervisorId",
                schema: "Core",
                table: "Clusters",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_ClusterKeepAlives_ClusterId",
                schema: "Core",
                table: "ClusterKeepAlives",
                column: "ClusterId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientServiceGroup_ServiceGroupId",
                schema: "Core",
                table: "ClientServiceGroup",
                column: "ServiceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientServiceGroup_ClientId_ServiceGroupId",
                schema: "Core",
                table: "ClientServiceGroup",
                columns: new[] { "ClientId", "ServiceGroupId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientServiceGroup_Clients_ClientId",
                schema: "Core",
                table: "ClientServiceGroup",
                column: "ClientId",
                principalSchema: "Core",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientServiceGroup_ServiceGroups_ServiceGroupId",
                schema: "Core",
                table: "ClientServiceGroup",
                column: "ServiceGroupId",
                principalSchema: "Core",
                principalTable: "ServiceGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClusterKeepAlives_Clusters_ClusterId",
                schema: "Core",
                table: "ClusterKeepAlives",
                column: "ClusterId",
                principalSchema: "Core",
                principalTable: "Clusters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clusters_Supervisors_SupervisorId",
                schema: "Core",
                table: "Clusters",
                column: "SupervisorId",
                principalSchema: "Core",
                principalTable: "Supervisors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClusterTokens_Clusters_Id",
                schema: "Core",
                table: "ClusterTokens",
                column: "Id",
                principalSchema: "Core",
                principalTable: "Clusters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingConfigurations_Supervisors_ModifyingId",
                schema: "Core",
                table: "ParkingConfigurations",
                column: "ModifyingId",
                principalSchema: "Core",
                principalTable: "Supervisors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingConfigurations_Parkings_ParkingId",
                schema: "Core",
                table: "ParkingConfigurations",
                column: "ParkingId",
                principalSchema: "Core",
                principalTable: "Parkings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingKeepAlives_Parkings_ParkingId",
                schema: "Core",
                table: "ParkingKeepAlives",
                column: "ParkingId",
                principalSchema: "Core",
                principalTable: "Parkings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingPlaces_Parkings_ParkingId",
                schema: "Core",
                table: "ParkingPlaces",
                column: "ParkingId",
                principalSchema: "Core",
                principalTable: "Parkings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingPlaces_ServiceGroups_ServiceGroupId",
                schema: "Core",
                table: "ParkingPlaces",
                column: "ServiceGroupId",
                principalSchema: "Core",
                principalTable: "ServiceGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parkings_Clusters_ClusterId",
                schema: "Core",
                table: "Parkings",
                column: "ClusterId",
                principalSchema: "Core",
                principalTable: "Clusters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionChange_SessionParkings_SessionParkingId",
                schema: "Core",
                table: "SessionChange",
                column: "SessionParkingId",
                principalSchema: "Core",
                principalTable: "SessionParkings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionParkings_ServiceGroups_ServiceGroupId",
                schema: "Core",
                table: "SessionParkings",
                column: "ServiceGroupId",
                principalSchema: "Core",
                principalTable: "ServiceGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
