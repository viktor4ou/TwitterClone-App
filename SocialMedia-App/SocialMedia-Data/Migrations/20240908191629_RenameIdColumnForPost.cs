using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameIdColumnForPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Comments",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                newName: "IX_Comments_Id");

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 9, 8, 22, 16, 29, 112, DateTimeKind.Local).AddTicks(6807));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 2,
                column: "DatePosted",
                value: new DateTime(2024, 9, 8, 22, 16, 29, 112, DateTimeKind.Local).AddTicks(6810));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 9, 8, 22, 16, 29, 112, DateTimeKind.Local).AddTicks(6691));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "DatePosted",
                value: new DateTime(2024, 9, 8, 22, 16, 29, 112, DateTimeKind.Local).AddTicks(6737));

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_Id",
                table: "Comments",
                column: "Id",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_Id",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Comments",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_Id",
                table: "Comments",
                newName: "IX_Comments_PostId");

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 9, 8, 22, 1, 53, 723, DateTimeKind.Local).AddTicks(9798));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 2,
                column: "DatePosted",
                value: new DateTime(2024, 9, 8, 22, 1, 53, 723, DateTimeKind.Local).AddTicks(9801));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 9, 8, 22, 1, 53, 723, DateTimeKind.Local).AddTicks(9663));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "DatePosted",
                value: new DateTime(2024, 9, 8, 22, 1, 53, 723, DateTimeKind.Local).AddTicks(9716));

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
