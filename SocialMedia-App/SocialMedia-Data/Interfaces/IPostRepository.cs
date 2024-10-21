using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.Models.Models;

namespace SocialMedia.Data.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        public Post GetPostById(int id);
        public void Update(Post entity);
    }
}
