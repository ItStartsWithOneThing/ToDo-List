using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ToDo_List.Models.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class removed_timeZone_from_RefreshSession_props : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiresIn",
                table: "RefreshSessions",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RefreshSessions",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.InsertData(
                table: "TaskCards",
                columns: new[] { "Id", "BackgroundColor", "Completed", "EditedDate", "Priority", "Text", "Title", "UserId" },
                values: new object[,]
                {
                    { new Guid("a92cb130-979f-465d-9019-6f64a524eb45"), "#ffb5a7", false, new DateTime(2024, 2, 10, 9, 17, 0, 0, DateTimeKind.Unspecified), 3, "Pay rent for electricity and water. Also send electricity meter readings.", "Rent", new Guid("ab4360f5-c721-4119-9908-6bf1a50aee08") },
                    { new Guid("aa09174a-866d-4c1f-8253-d29b02c92984"), "white", false, new DateTime(2024, 1, 2, 10, 30, 0, 0, DateTimeKind.Unspecified), 1, "Movies that I should watch this winter: Harry Potter (all parts), Lord of the ring + Hobbit (all parts), ", "Movies", new Guid("ab4360f5-c721-4119-9908-6bf1a50aee08") },
                    { new Guid("c477693d-5d2e-4f69-bf39-3c06b4b3f69a"), "#bde0fe", false, new DateTime(2024, 2, 5, 14, 30, 0, 0, DateTimeKind.Unspecified), 2, "Order some pizza", "Dinner", new Guid("ab4360f5-c721-4119-9908-6bf1a50aee08") },
                    { new Guid("f577f301-68f6-46a2-9a0c-09713d46812c"), "#d0f4de", false, new DateTime(2024, 2, 5, 14, 30, 0, 0, DateTimeKind.Unspecified), 2, "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.", "Basic text for filling", new Guid("ab4360f5-c721-4119-9908-6bf1a50aee08") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TaskCards",
                keyColumn: "Id",
                keyValue: new Guid("a92cb130-979f-465d-9019-6f64a524eb45"));

            migrationBuilder.DeleteData(
                table: "TaskCards",
                keyColumn: "Id",
                keyValue: new Guid("aa09174a-866d-4c1f-8253-d29b02c92984"));

            migrationBuilder.DeleteData(
                table: "TaskCards",
                keyColumn: "Id",
                keyValue: new Guid("c477693d-5d2e-4f69-bf39-3c06b4b3f69a"));

            migrationBuilder.DeleteData(
                table: "TaskCards",
                keyColumn: "Id",
                keyValue: new Guid("f577f301-68f6-46a2-9a0c-09713d46812c"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiresIn",
                table: "RefreshSessions",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RefreshSessions",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

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
        }
    }
}
