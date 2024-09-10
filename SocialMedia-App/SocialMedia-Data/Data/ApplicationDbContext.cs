using System;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Models.Models;

namespace SocialMedia.Data.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Posts
            modelBuilder.Entity<Post>().HasData(
                new Post
                {
                    Id = 1,
                    Content = "This is the content of the first post",
                    DatePosted = DateTime.Now
                },
                new Post
                {
                    Id = 2,
                    Content = "This is the content of the second post",
                    DatePosted = DateTime.Now
                }
            );

            // Seed data for Comments
            modelBuilder.Entity<Comment>().HasData(
                new Comment
                {
                    CommentId = 6,
                    Content = "This is the content of the first comment",
                    DatePosted = DateTime.Now,
                    PostId = 6 // Ensure this matches an existing PostId
                },
                new Comment
                {
                    CommentId = 7,
                    Content = "This is the content of the second comment",
                    DatePosted = DateTime.Now,
                    PostId =7 // Ensure this matches an existing PostId
                }
            );
        }
    }
}