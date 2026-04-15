using SocialMedia.Data.Interfaces;
using SocialMedia.Models.Models;
using SocialMedia.Models.ViewModels;
using SocialMedia_App.Services.Interfaces;

namespace SocialMedia_App.Services
{
    public class PostService : IPostService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ILIkeRepository _likeRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<PostService> _logger;

        public PostService(
            IWebHostEnvironment webHostEnvironment,
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            ILIkeRepository likeRepository,
            IUserRepository userRepository,
            ILogger<PostService> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _likeRepository = likeRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<PostViewModel> GetFeedViewModelAsync()
        {
            var viewModel = new PostViewModel
            {
                Post = new Post(),
                Posts = await _postRepository.GetAllAsync(),
                Comment = new Comment(),
                Comments = await _commentRepository.GetAllAsync(),
                Like = new Like(),
                Likes = await _likeRepository.GetAllAsync()
            };

            foreach (var post in viewModel.Posts)
            {
                post.TimeAgo = TimeAgo(post.DatePosted);
            }
            foreach (var comment in viewModel.Comments)
            {
                comment.TimeAgo = TimeAgo(comment.DatePosted);
            }

            return viewModel;
        }

        public async Task<IEnumerable<SearchResultViewModel>> SearchAsync(string query)
        {
            var users = await _userRepository.SearchByQueryAsync(query);
            var posts = await _postRepository.SearchByContentAsync(query);
            var postOwners = await _userRepository.GetByIdsAsync(posts.Select(p => p.PostOwnerId));
            var ownersById = postOwners.ToDictionary(u => u.Id);

            var userResults = users.Select(u => new SearchResultViewModel
            {
                Type = "user",
                Title = u.UserName ?? $"{u.FirstName} {u.LastName}".Trim(),
                Subtitle = $"{u.FirstName} {u.LastName}".Trim(),
                ImageUrl = u.ProfileImageURL,
                Url = $"/User/Profile?userId={u.Id}"
            });

            var postResults = posts.Select(p =>
            {
                ownersById.TryGetValue(p.PostOwnerId, out var owner);
                var ownerName = owner == null
                    ? "Unknown user"
                    : $"{owner.FirstName} {owner.LastName}".Trim();

                return new SearchResultViewModel
                {
                    Type = "post",
                    Title = TrimSearchText(p.Content, 80),
                    Subtitle = $"Post by {ownerName}",
                    ImageUrl = owner?.ProfileImageURL,
                    Url = $"/User/Home#post-{p.PostId}"
                };
            });

            return userResults.Concat(postResults);
        }

        public async Task CreatePostAsync(string userId, Post post, Stream? imageStream, string? imageExtension)
        {
            if (imageStream != null && imageExtension != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string filename = Guid.NewGuid().ToString() + imageExtension;
                string productPath = Path.Combine(wwwRootPath, "images", "posts");

                using (FileStream s = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                {
                    await imageStream.CopyToAsync(s);
                }
                post.ImageURL = Path.Combine("images", "posts", filename);
            }

            post.PostOwnerId = userId;
            post.DatePosted = DateTime.Now;

            await _postRepository.AddAsync(post);
            await _postRepository.SaveAsync();

            _logger.LogInformation("User {UserId} created post {PostId}", userId, post.PostId);
        }

        public async Task CreateCommentAsync(string userId, int postId, Comment comment)
        {
            comment.PostId = postId;
            comment.DatePosted = DateTime.Now;
            comment.CommentOwnerId = userId;

            await _commentRepository.AddAsync(comment);
            await _commentRepository.SaveAsync();

            _logger.LogInformation("User {UserId} commented on post {PostId}", userId, postId);
        }

        public async Task<OperationResult> DeletePostAsync(string userId, int postId)
        {
            Post searchedPost = await _postRepository.GetPostByIdAsync(postId);

            if (searchedPost == null || searchedPost.PostOwnerId != userId)
            {
                _logger.LogWarning("Unauthorized delete attempt on post {PostId} by user {UserId}", postId, userId);
                return OperationResult.Unauthorized;
            }

            List<Comment> postComments = await _commentRepository.GetCommentsByPostIdAsync(postId);

            if (searchedPost.ImageURL != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string imageFullPath = Path.Combine(wwwRootPath, searchedPost.ImageURL.TrimStart('\\'));
                System.IO.File.Delete(imageFullPath);
            }

            _postRepository.Remove(searchedPost);
            _commentRepository.RemoveRange(postComments);
            await _postRepository.SaveAsync();

            _logger.LogInformation("User {UserId} deleted post {PostId}", userId, postId);
            return OperationResult.Success;
        }

        public async Task<OperationResult> DeleteCommentAsync(string userId, int commentId, int postId)
        {
            Comment searchedComment = await _commentRepository.GetByIdAsync(commentId);
            Post searchedPost = await _postRepository.GetPostByIdAsync(postId);

            if (searchedComment == null || searchedPost == null)
            {
                _logger.LogWarning("Delete comment failed: comment {CommentId} or post {PostId} not found", commentId, postId);
                return OperationResult.NotFound;
            }

            if (searchedComment.PostId != postId)
            {
                return OperationResult.BadRequest;
            }

            if (searchedComment.CommentOwnerId != userId && userId != searchedPost.PostOwnerId)
            {
                _logger.LogWarning("User {UserId} unauthorized to delete comment {CommentId}", userId, commentId);
                return OperationResult.Unauthorized;
            }

            _commentRepository.Remove(searchedComment);
            await _commentRepository.SaveAsync();

            _logger.LogInformation("User {UserId} deleted comment {CommentId} on post {PostId}", userId, commentId, postId);
            return OperationResult.Success;
        }

        public async Task<Post?> GetPostForEditAsync(string userId, int postId)
        {
            Post post = await _postRepository.GetPostByIdAsync(postId);

            if (post == null || post.PostOwnerId != userId)
            {
                return null;
            }

            return post;
        }

        public async Task<OperationResult> EditPostAsync(string userId, int postId, string content, Stream? imageStream, string? imageExtension)
        {
            var originalPost = await _postRepository.GetPostByIdAsync(postId);

            if (originalPost == null)
            {
                return OperationResult.NotFound;
            }

            if (originalPost.PostOwnerId != userId)
            {
                return OperationResult.Unauthorized;
            }

            if (imageStream != null && imageExtension != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string filename = Guid.NewGuid().ToString() + imageExtension;
                string productPath = Path.Combine(wwwRootPath, "images", "posts");

                if (originalPost.ImageURL != null)
                {
                    string oldImagePath = Path.Combine(wwwRootPath, originalPost.ImageURL.TrimStart('\\'));
                    try
                    {
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to delete old image for post {PostId}", postId);
                    }
                }

                using (FileStream s = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                {
                    await imageStream.CopyToAsync(s);
                }

                originalPost.ImageURL = @"\images\posts\" + filename;
            }

            originalPost.Content = content;
            _postRepository.Update(originalPost);
            await _postRepository.SaveAsync();

            _logger.LogInformation("User {UserId} edited post {PostId}", userId, postId);
            return OperationResult.Success;
        }

        public async Task<OperationResult> ToggleLikeAsync(string userId, int postId)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);

            if (post == null)
            {
                return OperationResult.NotFound;
            }

            var like = await _likeRepository.GetByOwnerAndPostAsync(userId, postId);

            if (like == null)
            {
                like = new Like(postId, userId);
                await _likeRepository.AddAsync(like);
                post.Likes++;
                _logger.LogInformation("User {UserId} liked post {PostId}", userId, postId);
            }
            else
            {
                _likeRepository.Remove(like);
                post.Likes--;
                _logger.LogInformation("User {UserId} unliked post {PostId}", userId, postId);
            }

            _postRepository.Update(post);
            await _postRepository.SaveAsync();

            return OperationResult.Success;
        }

        private static string TimeAgo(DateTime postedDate)
        {
            var timeSpan = DateTime.Now - postedDate;

            return timeSpan switch
            {
                _ when timeSpan <= TimeSpan.FromSeconds(60) => "Just now",
                _ when timeSpan <= TimeSpan.FromMinutes(60) => $"{timeSpan.Minutes} minutes ago",
                _ when timeSpan <= TimeSpan.FromHours(24) => $"{timeSpan.Hours} hours ago",
                _ when timeSpan <= TimeSpan.FromDays(7) => $"{timeSpan.Days} days ago",
                _ when timeSpan <= TimeSpan.FromDays(30) => $"{timeSpan.Days / 7} weeks ago",
                _ => postedDate.ToString("MMMM dd, yyyy")
            };
        }

        private static string TrimSearchText(string value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length <= maxLength)
            {
                return value;
            }

            return value[..maxLength].TrimEnd() + "...";
        }
    }
}
