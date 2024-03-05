using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ToDo_List.Models.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class added_user_and_refreshSessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "TaskCards",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RefreshToken = table.Column<Guid>(type: "uuid", nullable: false),
                    FingerPrint = table.Column<string>(type: "text", nullable: false),
                    UserAgent = table.Column<string>(type: "text", nullable: false),
                    ExpiresIn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshSessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password" },
                values: new object[] { new Guid("ab4360f5-c721-4119-9908-6bf1a50aee08"), "example@test.com", "User", "$2a$11$RxAPh6vaKFA8SEmThlCaPuUY2GHd/gpa9eAvLn87E1YLPj6bXuii2" });

            migrationBuilder.InsertData(
                table: "TaskCards",
                columns: new[] { "Id", "BackgroundColor", "Completed", "EditedDate", "Priority", "Text", "Title", "UserId" },
                values: new object[,]
                {
                    { new Guid("ae332493-31bc-44ff-800d-b04b473618a5"), "#d0f4de", false, new DateTime(2024, 2, 5, 14, 30, 0, 0, DateTimeKind.Unspecified), 2, "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.", "Basic text for filling", new Guid("ab4360f5-c721-4119-9908-6bf1a50aee08") },
                    { new Guid("af47ccda-6c89-42fa-88e3-6a1a91fe81dc"), "#ffb5a7", false, new DateTime(2024, 2, 10, 9, 17, 0, 0, DateTimeKind.Unspecified), 3, "Pay rent for electricity and water. Also send electricity meter readings.", "Rent", new Guid("ab4360f5-c721-4119-9908-6bf1a50aee08") },
                    { new Guid("b7e882fe-55d5-44db-bb6c-d12f62351dc1"), "#bde0fe", false, new DateTime(2024, 2, 5, 14, 30, 0, 0, DateTimeKind.Unspecified), 2, "Order some pizza", "Dinner", new Guid("ab4360f5-c721-4119-9908-6bf1a50aee08") },
                    { new Guid("d282616b-3c96-4988-b64d-7be7e54c9a0d"), "white", false, new DateTime(2024, 1, 2, 10, 30, 0, 0, DateTimeKind.Unspecified), 1, "Movies that I should watch this winter: Harry Potter (all parts), Lord of the ring + Hobbit (all parts), ", "Movies", new Guid("ab4360f5-c721-4119-9908-6bf1a50aee08") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskCards_UserId",
                table: "TaskCards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshSessions_UserId",
                table: "RefreshSessions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskCards_Users_UserId",
                table: "TaskCards",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskCards_Users_UserId",
                table: "TaskCards");

            migrationBuilder.DropTable(
                name: "RefreshSessions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TaskCards_UserId",
                table: "TaskCards");

            migrationBuilder.DeleteData(
                table: "TaskCards",
                keyColumn: "Id",
                keyValue: new Guid("ae332493-31bc-44ff-800d-b04b473618a5"));

            migrationBuilder.DeleteData(
                table: "TaskCards",
                keyColumn: "Id",
                keyValue: new Guid("af47ccda-6c89-42fa-88e3-6a1a91fe81dc"));

            migrationBuilder.DeleteData(
                table: "TaskCards",
                keyColumn: "Id",
                keyValue: new Guid("b7e882fe-55d5-44db-bb6c-d12f62351dc1"));

            migrationBuilder.DeleteData(
                table: "TaskCards",
                keyColumn: "Id",
                keyValue: new Guid("d282616b-3c96-4988-b64d-7be7e54c9a0d"));

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TaskCards");
        }
    }
}
