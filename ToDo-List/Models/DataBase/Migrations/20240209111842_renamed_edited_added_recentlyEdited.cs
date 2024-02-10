using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo_List.Models.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class renamed_edited_added_recentlyEdited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Edited",
                table: "TaskCards",
                newName: "EditedDate");

            migrationBuilder.AddColumn<bool>(
                name: "RecentlyEdited",
                table: "TaskCards",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecentlyEdited",
                table: "TaskCards");

            migrationBuilder.RenameColumn(
                name: "EditedDate",
                table: "TaskCards",
                newName: "Edited");
        }
    }
}
