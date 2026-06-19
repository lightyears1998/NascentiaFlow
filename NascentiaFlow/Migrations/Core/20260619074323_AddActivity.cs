using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NascentiaFlow.Migrations.Core
{
    /// <inheritdoc />
    public partial class AddActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Focuses_ActivityType_ActivityTypeId",
                table: "Focuses");

            migrationBuilder.DropTable(
                name: "ActivityType");

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StartedAt = table.Column<string>(type: "TEXT", nullable: false),
                    EndedAt = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    FocusId = table.Column<Guid>(type: "TEXT", nullable: true),
                    EditionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EditionTimestamp = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_Focuses_FocusId",
                        column: x => x.FocusId,
                        principalTable: "Focuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ActivityTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    EditionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EditionTimestamp = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_FocusId",
                table: "Activities",
                column: "FocusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Focuses_ActivityTypes_ActivityTypeId",
                table: "Focuses",
                column: "ActivityTypeId",
                principalTable: "ActivityTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Focuses_ActivityTypes_ActivityTypeId",
                table: "Focuses");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "ActivityTypes");

            migrationBuilder.CreateTable(
                name: "ActivityType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    EditionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EditionTimestamp = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityType", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Focuses_ActivityType_ActivityTypeId",
                table: "Focuses",
                column: "ActivityTypeId",
                principalTable: "ActivityType",
                principalColumn: "Id");
        }
    }
}
