using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Data;
using SocialMedia.Data.Interfaces;
using SocialMedia.Models.Models;

namespace SocialMedia.Data.Repository
{
    public class FollowerRepository : Repository<Follower>, IFollowerRepository
    {
        private readonly ApplicationDbContext db;
        public FollowerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            db = dbContext;
        }

        public async Task<Follower> GetByOwnerAndFollowedAsync(string ownerId, string followedUserId)
        {
            return await db.Followers.FirstOrDefaultAsync(
                f => f.FollowOwnerId == ownerId &&
                f.FollowedUserId == followedUserId);
        }

        public async Task<List<Follower>> GetFollowersByUserIdAsync(string userId)
        {
            return await dbSet
                .Where(f => f.FollowOwnerId == userId)
                .ToListAsync();
        }
    }
}
