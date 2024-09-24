using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 9, 24, 22, 23, 43, 559, DateTimeKind.Local).AddTicks(3494));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 9, 24, 22, 23, 43, 559, DateTimeKind.Local).AddTicks(3324));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 9, 23, 21, 32, 26, 497, DateTimeKind.Local).AddTicks(1781));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 9, 23, 21, 32, 26, 497, DateTimeKind.Local).AddTicks(1610));
        }
    }
}
