using SocialMedia.Models.Models;

namespace SocialMedia.Data.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        public Task<Post> GetPostByIdAsync(int id);
        public Task<List<Post>> GetAllPostsByUserId(string id);
        public void Update(Post entity);
    }
}
