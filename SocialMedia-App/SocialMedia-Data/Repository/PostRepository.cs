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
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly ApplicationDbContext db;
        public PostRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            db = dbContext;
        }

        public Post GetPostById(int id)
        {
            return db.Posts.FirstOrDefault(i => i.PostId == id);
        }

        public void Update(Post entity)
        {
            db.Posts.Update(entity);
        }
    }
}
