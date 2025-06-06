using SocialMedia.Models.Models;

namespace SocialMedia.Data.Interfaces
{
    public interface IFollowerRepository : IRepository<Follower>
    {
        public Task<List<Follower>> GetFollowersByUserIdAsync(string userId);
        public Task<Follower> GetByOwnerAndFollowedAsync(string ownerId, string followedUserId);

    }
}
