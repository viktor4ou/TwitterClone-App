﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SocialMedia.Data.Data;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SocialMedia.Models.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentId"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DatePosted")
                        .HasColumnType("datetime2");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.HasKey("CommentId");

                    b.HasIndex("PostId");

                    b.ToTable("Comments");

                    b.HasData(
                        new
                        {
                            CommentId = 6,
                            Content = "This is the content of the first comment",
                            DatePosted = new DateTime(2024, 9, 9, 21, 51, 34, 621, DateTimeKind.Local).AddTicks(5579),
                            PostId = 6
                        },
                        new
                        {
                            CommentId = 7,
                            Content = "This is the content of the second comment",
                            DatePosted = new DateTime(2024, 9, 9, 21, 51, 34, 621, DateTimeKind.Local).AddTicks(5583),
                            PostId = 7
                        });
                });

            modelBuilder.Entity("SocialMedia.Models.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DatePosted")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Likes")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Posts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Content = "This is the content of the first post",
                            DatePosted = new DateTime(2024, 9, 9, 21, 51, 34, 621, DateTimeKind.Local).AddTicks(5366),
                            Likes = 0
                        },
                        new
                        {
                            Id = 2,
                            Content = "This is the content of the second post",
                            DatePosted = new DateTime(2024, 9, 9, 21, 51, 34, 621, DateTimeKind.Local).AddTicks(5432),
                            Likes = 0
                        });
                });

            modelBuilder.Entity("SocialMedia.Models.Models.Comment", b =>
                {
                    b.HasOne("SocialMedia.Models.Models.Post", "Post")
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });
#pragma warning restore 612, 618
        }
    }
}
