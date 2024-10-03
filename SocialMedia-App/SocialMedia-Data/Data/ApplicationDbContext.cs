using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Models.Models;

namespace SocialMedia.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Like> Likes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Posts
            modelBuilder.Entity<Post>().HasData(
                new Post { PostId = 1, Content = "First Post", DatePosted = DateTime.Now, PostOwnerId = "611a46b0-33d0-4609-b43d-f2b47617792b" }
            );

            // Seed data for Comments
            modelBuilder.Entity<Comment>().HasData(
                new Comment { CommentId = 1, Content = "First Comment", DatePosted = DateTime.Now, PostId = 1, CommentOwnerId = "611a46b0-33d0-4609-b43d-f2b47617792b" }
            );
        }
        
    }
}