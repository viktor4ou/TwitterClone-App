using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Models.Models;

namespace SocialMedia.Data.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }
        public DbSet<Post> Posts  { get; set; }
        public DbSet<Comment> Comments  { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
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
            modelBuilder.Entity<Comment>().HasData(
                new Comment
                {
                    CommentId = 1,
                    Content = "This is the content of the first comment",
                    DatePosted = DateTime.Now,
                    Id = 1
                },
                new Comment
                {
                    CommentId = 2,
                    Content = "This is the content of the second comment",
                    DatePosted = DateTime.Now,
                    Id = 2
                }
            );
        }
        
    }
}
