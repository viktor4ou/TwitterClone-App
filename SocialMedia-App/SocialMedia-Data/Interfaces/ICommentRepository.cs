using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.Models.Models;

namespace SocialMedia.Data.Interfaces
{
    public interface ICommentRepository: IRepository<Comment>
    {
        public void RemoveRange(List<Comment> comments);
        public Comment GetById(int commentId);//Check if we to route the commentid
    }
}
