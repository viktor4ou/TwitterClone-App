using SocialMedia.Models.Models;

namespace SocialMedia.Data.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        public Task<Post> GetPostByIdAsync(int id);
        public Task<List<Post>> GetAllPostsByUserId(string id);
        public Task<List<Post>> SearchByContentAsync(string query, int take = 5);
        public void Update(Post entity);
    }
}
