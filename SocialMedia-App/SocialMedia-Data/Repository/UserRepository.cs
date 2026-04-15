using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Data;
using SocialMedia.Data.Interfaces;
using SocialMedia.Models.Models;

namespace SocialMedia.Data.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext db;
        public UserRepository(ApplicationDbContext dbContext) :
            base(dbContext)
        {
            db = dbContext;
        }

        public async Task<List<ApplicationUser>> SearchByQueryAsync(string query, int take = 5)
        {
            var normalizedQuery = query.Trim().ToLower();

            var users = await db.Users
                .Where(u =>
                    (u.UserName != null && u.UserName.ToLower().Contains(normalizedQuery)) ||
                    u.FirstName.ToLower().Contains(normalizedQuery) ||
                    u.LastName.ToLower().Contains(normalizedQuery) ||
                    (u.FirstName + " " + u.LastName).ToLower().Contains(normalizedQuery))
                .OrderBy(u => u.UserName)
                .Take(take)
                .ToListAsync();

            return users;
        }

        public async Task<List<ApplicationUser>> GetByIdsAsync(IEnumerable<string> userIds)
        {
            var distinctIds = userIds
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct()
                .ToList();

            return await db.Users
                .Where(u => distinctIds.Contains(u.Id))
                .ToListAsync();
        }
    }
}
