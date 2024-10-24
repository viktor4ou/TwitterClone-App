using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.Data.Data;
using SocialMedia.Data.Interfaces;
using SocialMedia.Models.Models;

namespace SocialMedia.Data.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext db;
        public UserRepository(ApplicationDbContext dbContext):
            base(dbContext)
        {
            db = dbContext;
        }
    }
}
