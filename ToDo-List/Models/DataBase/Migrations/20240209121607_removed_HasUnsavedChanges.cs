using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo_List.Models.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class removed_HasUnsavedChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasUnsavedChanges",
                table: "TaskCards");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasUnsavedChanges",
                table: "TaskCards",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
