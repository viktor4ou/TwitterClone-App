using SocialMedia.Models.Models;
using SocialMedia.Models.ViewModels;

namespace SocialMedia_App.Services.Interfaces
{
    public interface IPostService
    {
        Task<PostViewModel> GetFeedViewModelAsync();
        Task<IEnumerable<SearchResultViewModel>> SearchAsync(string query);
        Task CreatePostAsync(string userId, Post post, Stream? imageStream, string? imageExtension);
        Task CreateCommentAsync(string userId, int postId, Comment comment);
        Task<OperationResult> DeletePostAsync(string userId, int postId);
        Task<OperationResult> DeleteCommentAsync(string userId, int commentId, int postId);
        Task<Post?> GetPostForEditAsync(string userId, int postId);
        Task<OperationResult> EditPostAsync(string userId, int postId, string content, Stream? imageStream, string? imageExtension);
        Task<OperationResult> ToggleLikeAsync(string userId, int postId);
    }
}
