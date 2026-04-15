using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models.Models;
using SocialMedia.Models.ViewModels;
using SocialMedia_App.Services;
using SocialMedia_App.Services.Interfaces;
using System.Diagnostics;

namespace SocialMedia_App.Controllers
{
    [Authorize]
    public abstract class HomeControllerBase : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPostService _postService;
        private readonly ILogger<HomeControllerBase> _logger;

        protected HomeControllerBase(
            SignInManager<ApplicationUser> signInManager,
            IPostService postService,
            ILogger<HomeControllerBase> logger)
        {
            _signInManager = signInManager;
            _postService = postService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Feed loaded by {User}", User.Identity?.Name ?? "anonymous");
            PostViewModel viewModel = await _postService.GetFeedViewModelAsync();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
            {
                _logger.LogWarning("Unauthenticated search attempt for query '{Query}'", query);
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new List<object>());
            }

            _logger.LogInformation("User {UserId} searched for '{Query}'", currentUser.Id, query);
            var results = await _postService.SearchAsync(query);
            return Json(results);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostViewModel postVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await GetCurrentUserAsync();

                if (currentUser == null)
                {
                    _logger.LogWarning("Unauthenticated attempt to create a post");
                    return Unauthorized();
                }

                Stream? imageStream = file?.OpenReadStream();
                string? imageExtension = file != null ? Path.GetExtension(file.FileName) : null;
                await _postService.CreatePostAsync(currentUser.Id, postVM.Post, imageStream, imageExtension);
                return RedirectToAction("Index");
            }
            ViewData["ShowModal"] = true;
            return View("Index", postVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(int postId, PostViewModel postVM)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
            {
                _logger.LogWarning("Unauthenticated attempt to comment on post {PostId}", postId);
                return Unauthorized();
            }

            await _postService.CreateCommentAsync(currentUser.Id, postId, postVM.Comment);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeletePost(int id)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null) return Unauthorized();

            var result = await _postService.DeletePostAsync(currentUser.Id, id);
            if (result != OperationResult.Success) return Unauthorized();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteComment(int commentId, int postId)
        {
            if (commentId <= 0 || postId <= 0)
            {
                return BadRequest("Comment ID and Post ID must be greater than zero.");
            }

            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
            {
                _logger.LogWarning("Unauthenticated attempt to delete comment {CommentId}", commentId);
                return Unauthorized();
            }

            var result = await _postService.DeleteCommentAsync(currentUser.Id, commentId, postId);
            return result switch
            {
                OperationResult.Success => RedirectToAction("Index"),
                OperationResult.NotFound => NotFound("Comment or Post not found."),
                OperationResult.BadRequest => BadRequest("Comment does not belong to the specified post."),
                OperationResult.Unauthorized => Unauthorized(),
                _ => BadRequest()
            };
        }

        public async Task<IActionResult> EditPost(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Post ID must be greater than zero.");
            }

            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null) return Unauthorized();

            var post = await _postService.GetPostForEditAsync(currentUser.Id, id);
            if (post == null) return NotFound("Post not found.");

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(Post editedPost, IFormFile? file)
        {
            if (editedPost == null)
            {
                return BadRequest("Post data is required.");
            }

            if (editedPost.PostId <= 0)
            {
                return BadRequest("Post ID must be greater than zero.");
            }

            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null) return Unauthorized();

            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(editedPost.Content))
                {
                    ModelState.AddModelError("Content", "Post content cannot be empty.");
                    return View("EditPost", editedPost);
                }

                if (file != null && file.Length == 0)
                {
                    ModelState.AddModelError("file", "Please select a file to upload.");
                    return View("EditPost", editedPost);
                }

                Stream? imageStream = file?.OpenReadStream();
                string? imageExtension = file != null ? Path.GetExtension(file.FileName) : null;

                var result = await _postService.EditPostAsync(currentUser.Id, editedPost.PostId, editedPost.Content, imageStream, imageExtension);
                return result switch
                {
                    OperationResult.Success => RedirectToAction("Index"),
                    OperationResult.NotFound => NotFound("Post not found."),
                    _ => Unauthorized()
                };
            }
            return View("EditPost", editedPost);
        }

        [HttpPost]
        public async Task<IActionResult> LikePost(int postId)
        {
            if (postId <= 0)
            {
                return BadRequest("Post ID must be greater than zero.");
            }

            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null) return Unauthorized();

            var result = await _postService.ToggleLikeAsync(currentUser.Id, postId);
            if (result == OperationResult.NotFound) return NotFound("Post not found.");

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<ApplicationUser?> GetCurrentUserAsync()
        {
            return await _signInManager.UserManager.GetUserAsync(User);
        }
    }
}
