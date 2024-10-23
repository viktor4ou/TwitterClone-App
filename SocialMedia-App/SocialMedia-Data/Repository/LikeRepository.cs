using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.Data.Data;
using SocialMedia.Data.Interfaces;
using SocialMedia.Models.Models;

namespace SocialMedia.Data.Repository
{
    public class LikeRepository : Repository<Like>, ILIkeRepository
    {
        private readonly ApplicationDbContext db;
        public LikeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            db = dbContext;
        }
    }
}
