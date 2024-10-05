using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateLikesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    LikeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    LikeOwnerId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.LikeId);
                });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 10, 5, 19, 29, 11, 621, DateTimeKind.Local).AddTicks(9201));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 10, 5, 19, 29, 11, 621, DateTimeKind.Local).AddTicks(9017));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 9, 30, 21, 23, 40, 964, DateTimeKind.Local).AddTicks(2090));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 9, 30, 21, 23, 40, 964, DateTimeKind.Local).AddTicks(1853));
        }
    }
}
