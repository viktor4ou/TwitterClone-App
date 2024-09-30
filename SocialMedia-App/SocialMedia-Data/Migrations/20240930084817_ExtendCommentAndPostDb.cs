using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendCommentAndPostDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostOwnerId",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CommentOwnerId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                columns: new[] { "CommentOwnerId", "DatePosted" },
                values: new object[] { "swgfsdfg", new DateTime(2024, 9, 30, 11, 48, 15, 981, DateTimeKind.Local).AddTicks(6510) });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 1,
                columns: new[] { "DatePosted", "PostOwnerId" },
                values: new object[] { new DateTime(2024, 9, 30, 11, 48, 15, 981, DateTimeKind.Local).AddTicks(6208), "swgfsdfg" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostOwnerId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CommentOwnerId",
                table: "Comments");

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 9, 29, 1, 41, 38, 424, DateTimeKind.Local).AddTicks(1410));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 9, 29, 1, 41, 38, 424, DateTimeKind.Local).AddTicks(1222));
        }
    }
}
