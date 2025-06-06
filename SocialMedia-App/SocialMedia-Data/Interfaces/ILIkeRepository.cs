using SocialMedia.Models.Models;

namespace SocialMedia.Data.Interfaces
{
    public interface ILIkeRepository : IRepository<Like>
    {
        Task<Like> GetByOwnerAndPostAsync(string ownerId, int postId);
    }
}
