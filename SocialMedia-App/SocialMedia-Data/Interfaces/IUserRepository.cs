using SocialMedia.Models.Models;

namespace SocialMedia.Data.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        public Task<List<ApplicationUser>> SearchByUsername(string query);
    }
}
