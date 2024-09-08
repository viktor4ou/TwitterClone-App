using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatePosted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "CommentId", "Content", "DatePosted", "PostId" },
                values: new object[,]
                {
                    { 1, "This is the content of the first comment", new DateTime(2024, 9, 8, 22, 1, 53, 723, DateTimeKind.Local).AddTicks(9798), 1 },
                    { 2, "This is the content of the second comment", new DateTime(2024, 9, 8, 22, 1, 53, 723, DateTimeKind.Local).AddTicks(9801), 2 }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "DatePosted",
                value: new DateTime(2024, 9, 8, 21, 57, 40, 518, DateTimeKind.Local).AddTicks(9382));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "DatePosted",
                value: new DateTime(2024, 9, 8, 21, 57, 40, 518, DateTimeKind.Local).AddTicks(9430));
        }
    }
}
