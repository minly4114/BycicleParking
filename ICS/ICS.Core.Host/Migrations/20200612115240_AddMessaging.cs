using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ICS.Core.Host.Migrations
{
    public partial class AddMessaging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dialogs",
                schema: "Core",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(nullable: false),
                    SessionUuid = table.Column<Guid>(nullable: true),
                    CraetedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dialogs", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_Dialogs_SessionParkings_SessionUuid",
                        column: x => x.SessionUuid,
                        principalSchema: "Core",
                        principalTable: "SessionParkings",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Participant",
                schema: "Core",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    DialogUuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participant", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_Participant_Dialogs_DialogUuid",
                        column: x => x.DialogUuid,
                        principalSchema: "Core",
                        principalTable: "Dialogs",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DialogUuid = table.Column<Guid>(nullable: true),
                    SenderUuid = table.Column<Guid>(nullable: true),
                    RecipientUuid = table.Column<Guid>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Read = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Dialogs_DialogUuid",
                        column: x => x.DialogUuid,
                        principalSchema: "Core",
                        principalTable: "Dialogs",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Participant_RecipientUuid",
                        column: x => x.RecipientUuid,
                        principalSchema: "Core",
                        principalTable: "Participant",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Participant_SenderUuid",
                        column: x => x.SenderUuid,
                        principalSchema: "Core",
                        principalTable: "Participant",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_SessionUuid",
                schema: "Core",
                table: "Dialogs",
                column: "SessionUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_DialogUuid",
                schema: "Core",
                table: "Messages",
                column: "DialogUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecipientUuid",
                schema: "Core",
                table: "Messages",
                column: "RecipientUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderUuid",
                schema: "Core",
                table: "Messages",
                column: "SenderUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_DialogUuid",
                schema: "Core",
                table: "Participant",
                column: "DialogUuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "Participant",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "Dialogs",
                schema: "Core");
        }
    }
}
