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
    public class FollowerRepository : Repository<Follower>, IFollowerRepository
    {
        private readonly ApplicationDbContext db;
        public FollowerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            db = dbContext;
        }
    }
}
