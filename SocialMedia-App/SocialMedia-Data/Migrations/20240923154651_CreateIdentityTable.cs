using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateIdentityTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropIndex(
                name: "IX_Comments_PostId",
                table: "Comments");

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                columns: new[] { "Content", "DatePosted" },
                values: new object[] { "First Comment", new DateTime(2024, 9, 23, 18, 46, 51, 85, DateTimeKind.Local).AddTicks(8017) });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 1,
                columns: new[] { "Content", "DatePosted" },
                values: new object[] { "First Post", new DateTime(2024, 9, 23, 18, 46, 51, 85, DateTimeKind.Local).AddTicks(7646) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                columns: new[] { "Content", "DatePosted" },
                values: new object[] { "This is the content of the first comment", new DateTime(2024, 9, 23, 18, 31, 44, 802, DateTimeKind.Local).AddTicks(5129) });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 1,
                columns: new[] { "Content", "DatePosted" },
                values: new object[] { "This is the content of the first post", new DateTime(2024, 9, 23, 18, 31, 44, 802, DateTimeKind.Local).AddTicks(4979) });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "PostId", "Content", "DatePosted", "ImageURL", "Likes" },
                values: new object[] { 2, "This is the content of the second post", new DateTime(2024, 9, 23, 18, 31, 44, 802, DateTimeKind.Local).AddTicks(5021), null, 0 });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "CommentId", "Content", "DatePosted", "PostId" },
                values: new object[] { 2, "This is the content of the second comment", new DateTime(2024, 9, 23, 18, 31, 44, 802, DateTimeKind.Local).AddTicks(5133), 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            
        }
    }
}
