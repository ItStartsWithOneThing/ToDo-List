using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo_List.Models.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class removed_text_color : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TextColor",
                table: "TaskCards");

            migrationBuilder.DropColumn(
                name: "TitleColor",
                table: "TaskCards");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "TaskCards",
                newName: "Edited");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TaskCards",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Edited",
                table: "TaskCards",
                newName: "CreatedDate");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TaskCards",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextColor",
                table: "TaskCards",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitleColor",
                table: "TaskCards",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
