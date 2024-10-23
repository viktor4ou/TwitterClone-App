using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.Data.Data;
using SocialMedia.Data.Interfaces;
using SocialMedia.Models.Models;

namespace SocialMedia.Data.Repository
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private readonly ApplicationDbContext db;
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
            db = context;
        }

        public Comment GetById(int commentId)
        {
            return dbSet.FirstOrDefault(x=>x.CommentId == commentId);
        }

        public void RemoveRange(List<Comment> comments)
        {
            dbSet.RemoveRange(comments);
        }
    } 
}
