using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo_List.Models.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class renamed_edited_on_HasUnsaveChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecentlyEdited",
                table: "TaskCards",
                newName: "HasUnsavedChanges");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasUnsavedChanges",
                table: "TaskCards",
                newName: "RecentlyEdited");
        }
    }
}
