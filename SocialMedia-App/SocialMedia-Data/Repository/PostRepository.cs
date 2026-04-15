using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Data;
using SocialMedia.Data.Interfaces;
using SocialMedia.Models.Models;

namespace SocialMedia.Data.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly ApplicationDbContext db;
        public PostRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            db = dbContext;
        }

        public async Task<List<Post>> GetAllPostsByUserId(string id)
        {
            return await db.Posts.Where(p => p.PostOwnerId == id).ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await db.Posts.FirstOrDefaultAsync(i => i.PostId == id);
        }

        public async Task<List<Post>> SearchByContentAsync(string query, int take = 5)
        {
            var normalizedQuery = query.Trim().ToLower();

            return await db.Posts
                .Where(p => p.Content.ToLower().Contains(normalizedQuery))
                .OrderByDescending(p => p.DatePosted)
                .Take(take)
                .ToListAsync();
        }

        public void Update(Post entity)
        {
            db.Posts.Update(entity);
        }
    }
}
