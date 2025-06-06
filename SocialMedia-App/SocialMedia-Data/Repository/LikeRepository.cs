using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Data;
using SocialMedia.Data.Interfaces;
using SocialMedia.Models.Models;

namespace SocialMedia.Data.Repository
{
    public class LikeRepository : Repository<Like>, ILIkeRepository
    {
        private readonly ApplicationDbContext db;
        public LikeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            db = dbContext;
        }
        public async Task<Like> GetByOwnerAndPostAsync(string ownerId, int postId)
        {
            return await db.Likes.FirstOrDefaultAsync(l => l.LikeOwnerId == ownerId && l.PostId == postId);
        }
    }
}
