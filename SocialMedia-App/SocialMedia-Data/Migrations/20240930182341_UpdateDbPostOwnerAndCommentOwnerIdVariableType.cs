using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbPostOwnerAndCommentOwnerIdVariableType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PostOwnerId",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "CommentOwnerId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                columns: new[] { "CommentOwnerId", "DatePosted" },
                values: new object[] { "611a46b0-33d0-4609-b43d-f2b47617792b", new DateTime(2024, 9, 30, 21, 23, 40, 964, DateTimeKind.Local).AddTicks(2090) });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 1,
                columns: new[] { "DatePosted", "PostOwnerId" },
                values: new object[] { new DateTime(2024, 9, 30, 21, 23, 40, 964, DateTimeKind.Local).AddTicks(1853), "611a46b0-33d0-4609-b43d-f2b47617792b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PostOwnerId",
                table: "Posts",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "CommentOwnerId",
                table: "Comments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                columns: new[] { "CommentOwnerId", "DatePosted" },
                values: new object[] { 1, new DateTime(2024, 9, 30, 21, 15, 56, 192, DateTimeKind.Local).AddTicks(9239) });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 1,
                columns: new[] { "DatePosted", "PostOwnerId" },
                values: new object[] { new DateTime(2024, 9, 30, 21, 15, 56, 192, DateTimeKind.Local).AddTicks(9075), 1 });
        }
    }
}
