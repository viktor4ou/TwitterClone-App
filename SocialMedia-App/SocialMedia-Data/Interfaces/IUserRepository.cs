using SocialMedia.Models.Models;

namespace SocialMedia.Data.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        public Task<List<ApplicationUser>> SearchByQueryAsync(string query, int take = 5);
        public Task<List<ApplicationUser>> GetByIdsAsync(IEnumerable<string> userIds);
    }
}
