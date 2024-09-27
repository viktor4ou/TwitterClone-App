using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 9, 27, 23, 45, 4, 923, DateTimeKind.Local).AddTicks(1933));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 9, 27, 23, 45, 4, 923, DateTimeKind.Local).AddTicks(1706));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 9, 26, 0, 6, 26, 127, DateTimeKind.Local).AddTicks(1431));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 9, 26, 0, 6, 26, 127, DateTimeKind.Local).AddTicks(1214));
        }
    }
}
