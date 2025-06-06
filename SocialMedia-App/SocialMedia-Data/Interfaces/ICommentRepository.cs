using SocialMedia.Models.Models;

namespace SocialMedia.Data.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        public void RemoveRange(List<Comment> comments);
        public Task<Comment> GetByIdAsync(int commentId);
        public Task<List<Comment>> GetCommentsByPostIdAsync(int postId);
    }
}
