using Microsoft.EntityFrameworkCore;
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

        public async Task<Comment> GetByIdAsync(int commentId)
        {
            return await dbSet.FirstOrDefaultAsync(x => x.CommentId == commentId);
        }

        public async Task<List<Comment>> GetCommentsByPostIdAsync(int postId)
        {
            return await dbSet.Where(c => c.PostId == postId).ToListAsync();
        }

        public void RemoveRange(List<Comment> comments)
        {
            dbSet.RemoveRange(comments);
        }
    }
}
