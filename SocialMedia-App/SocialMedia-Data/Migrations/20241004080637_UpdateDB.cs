using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 10, 4, 11, 6, 36, 9, DateTimeKind.Local).AddTicks(7438));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 10, 4, 11, 6, 36, 9, DateTimeKind.Local).AddTicks(7149));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 10, 3, 21, 14, 15, 200, DateTimeKind.Local).AddTicks(3587));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 10, 3, 21, 14, 15, 200, DateTimeKind.Local).AddTicks(3397));
        }
    }
}
