using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NascentiaFlow.Migrations.Core
{
    /// <inheritdoc />
    public partial class ModifyActivityTableAddNameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("Description", "Activities", "Name");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Activities",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Activities");

            migrationBuilder.RenameColumn("Description", "Activities", "Name");
        }
    }
}
