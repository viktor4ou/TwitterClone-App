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

        public async Task<List<ApplicationUser>> SearchByUsername(string query)
        {
            var users = await db.Users.Where(u => u.UserName.Contains(query))
                .Take(10)
                .ToListAsync();
            return users;

        }
    }
}
