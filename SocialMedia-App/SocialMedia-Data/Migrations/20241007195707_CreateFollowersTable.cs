using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateFollowersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Followers",
                columns: table => new
                {
                    FollowerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FollowOwnerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FollowedUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Followers", x => x.FollowerId);
                });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 10, 7, 22, 57, 7, 320, DateTimeKind.Local).AddTicks(7367));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 10, 7, 22, 57, 7, 320, DateTimeKind.Local).AddTicks(7181));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Followers");

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
    }
}
