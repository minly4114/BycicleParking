using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ICS.Core.Host.Migrations
{
    public partial class ChangeMessaging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Participant_RecipientUuid",
                schema: "Core",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Participant_SenderUuid",
                schema: "Core",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Participant_Dialogs_DialogUuid",
                schema: "Core",
                table: "Participant");

            migrationBuilder.DropIndex(
                name: "IX_Messages_RecipientUuid",
                schema: "Core",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participant",
                schema: "Core",
                table: "Participant");

            migrationBuilder.DropIndex(
                name: "IX_Participant_DialogUuid",
                schema: "Core",
                table: "Participant");

            migrationBuilder.DropColumn(
                name: "Read",
                schema: "Core",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "RecipientUuid",
                schema: "Core",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "DialogUuid",
                schema: "Core",
                table: "Participant");

            migrationBuilder.RenameTable(
                name: "Participant",
                schema: "Core",
                newName: "Participants",
                newSchema: "Core");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participants",
                schema: "Core",
                table: "Participants",
                column: "Uuid");

            migrationBuilder.CreateTable(
                name: "DialogParticipant",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DialogUuid = table.Column<Guid>(nullable: false),
                    ParticipantUuid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DialogParticipant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DialogParticipant_Dialogs_DialogUuid",
                        column: x => x.DialogUuid,
                        principalSchema: "Core",
                        principalTable: "Dialogs",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DialogParticipant_Participants_ParticipantUuid",
                        column: x => x.ParticipantUuid,
                        principalSchema: "Core",
                        principalTable: "Participants",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Read",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UuidParticipant = table.Column<Guid>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false),
                    MessageId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Read", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Read_Messages_MessageId",
                        column: x => x.MessageId,
                        principalSchema: "Core",
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DialogParticipant_ParticipantUuid",
                schema: "Core",
                table: "DialogParticipant",
                column: "ParticipantUuid");

            migrationBuilder.CreateIndex(
                name: "IX_DialogParticipant_DialogUuid_ParticipantUuid",
                schema: "Core",
                table: "DialogParticipant",
                columns: new[] { "DialogUuid", "ParticipantUuid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Read_MessageId",
                schema: "Core",
                table: "Read",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Participants_SenderUuid",
                schema: "Core",
                table: "Messages",
                column: "SenderUuid",
                principalSchema: "Core",
                principalTable: "Participants",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Participants_SenderUuid",
                schema: "Core",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "DialogParticipant",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "Read",
                schema: "Core");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participants",
                schema: "Core",
                table: "Participants");

            migrationBuilder.RenameTable(
                name: "Participants",
                schema: "Core",
                newName: "Participant",
                newSchema: "Core");

            migrationBuilder.AddColumn<bool>(
                name: "Read",
                schema: "Core",
                table: "Messages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "RecipientUuid",
                schema: "Core",
                table: "Messages",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DialogUuid",
                schema: "Core",
                table: "Participant",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participant",
                schema: "Core",
                table: "Participant",
                column: "Uuid");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecipientUuid",
                schema: "Core",
                table: "Messages",
                column: "RecipientUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_DialogUuid",
                schema: "Core",
                table: "Participant",
                column: "DialogUuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Participant_RecipientUuid",
                schema: "Core",
                table: "Messages",
                column: "RecipientUuid",
                principalSchema: "Core",
                principalTable: "Participant",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Participant_SenderUuid",
                schema: "Core",
                table: "Messages",
                column: "SenderUuid",
                principalSchema: "Core",
                principalTable: "Participant",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participant_Dialogs_DialogUuid",
                schema: "Core",
                table: "Participant",
                column: "DialogUuid",
                principalSchema: "Core",
                principalTable: "Dialogs",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
