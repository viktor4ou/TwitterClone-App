using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.Interfaces;
using SocialMedia.Models.Models;
using SocialMedia.Models.ViewModels;
using System.Diagnostics;
namespace SocialMedia_App.Controllers
{
    [Authorize]
    public abstract class HomeControllerBase : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IPostRepository postRepository;
        private readonly ICommentRepository commentRepository;
        private readonly ILIkeRepository likeRepository;
        private readonly IUserRepository userRepository;

        protected HomeControllerBase(
            IWebHostEnvironment webHostEnvironment,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            ILIkeRepository likeRepository,
            IUserRepository userRepository)
        {
            this.commentRepository = commentRepository;
            this.likeRepository = likeRepository;
            this.postRepository = postRepository;
            this.userRepository = userRepository;
            this.webHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            PostViewModel viewModel = await GetViewModel();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            var currentLoggedUser = await GetCurrentLoggedUserAsync(useSignInManager: true);

            if (currentLoggedUser == null)
            {
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new List<object>());
            }

            var users = await userRepository.SearchByUsername(query);

            return Json(users.Select(u => new { u.UserName, u.Id, u.ProfileImageURL }));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostViewModel postVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var currentLoggedUser = await GetCurrentLoggedUserAsync(useSignInManager: true);

                if (currentLoggedUser == null)
                {
                    return Unauthorized();
                }

                if (file != null)
                {
                    string wwwRootPath = webHostEnvironment.WebRootPath;
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, "images", "posts");

                    using (FileStream s = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(s);
                    }
                    postVM.Post.ImageURL = Path.Combine("images", "posts", filename);
                }

                postVM.Post.PostOwnerId = currentLoggedUser.Id;
                postVM.Post.DatePosted = DateTime.Now;

                await postRepository.AddAsync(postVM.Post);
                await postRepository.SaveAsync();

                return RedirectToAction("Index");
            }
            ViewData["ShowModal"] = true;

            return View("Index", postVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(int postId, PostViewModel postVM)
        {
            var currentLoggedUser = await GetCurrentLoggedUserAsync(useSignInManager: true);

            if (currentLoggedUser == null)
            {
                return Unauthorized();
            }

            postVM.Comment.PostId = postId;
            postVM.Comment.DatePosted = DateTime.Now;
            postVM.Comment.CommentOwnerId = currentLoggedUser.Id;

            await commentRepository.AddAsync(postVM.Comment);
            await commentRepository.SaveAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeletePost(int id)
        {
            var user = await GetCurrentLoggedUserAsync(useSignInManager: false);

            Post searchedPost = await postRepository.GetPostByIdAsync(id);

            if (searchedPost == null || user == null || searchedPost.PostOwnerId != user.Id)
            {
                return Unauthorized();
            }

            List<Comment> postComments = await commentRepository.GetCommentsByPostIdAsync(id);

            // Delete existing image
            if (searchedPost.ImageURL != null)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;
                string imageFullPath = Path.Combine(wwwRootPath, searchedPost.ImageURL.TrimStart('\\'));
                System.IO.File.Delete(imageFullPath);
            }

            postRepository.Remove(searchedPost);
            commentRepository.RemoveRange(postComments);
            await postRepository.SaveAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteComment(int commentId, int postId)
        {
            if (commentId <= 0 || postId <= 0)
            {
                return BadRequest("Comment ID and Post ID must be greater than zero.");
            }

            var currentLoggedUser = await GetCurrentLoggedUserAsync(useSignInManager: true);

            if (currentLoggedUser == null)
            {
                return Unauthorized();
            }

            Comment searchedComment = await commentRepository.GetByIdAsync(commentId);
            Post searchedPost = await postRepository.GetPostByIdAsync(postId);

            if (searchedComment == null || searchedPost == null)
            {
                return NotFound("Comment or Post not found.");
            }

            if (searchedComment.PostId != postId)
            {
                return BadRequest("Comment does not belong to the specified post.");
            }

            if ((searchedComment.CommentOwnerId != currentLoggedUser.Id) && (currentLoggedUser.Id != searchedPost.PostOwnerId))
            {
                return Unauthorized();
            }

            commentRepository.Remove(searchedComment);
            await commentRepository.SaveAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditPost(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Post ID must be greater than zero.");
            }

            var currentLoggedUser = await GetCurrentLoggedUserAsync(useSignInManager: false);

            if (currentLoggedUser == null)
            {
                return Unauthorized();
            }

            PostViewModel viewModel = await GetViewModel();
            viewModel.Post = await postRepository.GetPostByIdAsync(id);

            if (viewModel.Post == null)
            {
                return NotFound("Post not found.");
            }

            // Validate authorization: User must be the post owner
            if (viewModel.Post.PostOwnerId != currentLoggedUser.Id)
            {
                return Unauthorized();
            }

            return View(viewModel.Post);
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

            var currentLoggedUser = await GetCurrentLoggedUserAsync(useSignInManager: false);

            if (currentLoggedUser == null)
            {
                return Unauthorized();
            }

            var originalPost = await postRepository.GetPostByIdAsync(editedPost.PostId);

            if (originalPost == null)
            {
                return NotFound("Post not found.");
            }

            if (originalPost.PostOwnerId != currentLoggedUser.Id)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(editedPost.Content))
                {
                    ModelState.AddModelError("Content", "Post content cannot be empty.");
                    return View("EditPost", editedPost);
                }

                // Uploaded a new image
                if (file != null)
                {
                    if (file.Length == 0)
                    {
                        ModelState.AddModelError("file", "Please select a file to upload.");
                        return View("EditPost", editedPost);
                    }

                    string wwwRootPath = webHostEnvironment.WebRootPath;
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, "images", "posts");

                    // Delete existing image
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
                            // Log or handle file deletion error, but continue
                            System.Diagnostics.Debug.WriteLine($"Error deleting old image: {ex.Message}");
                        }
                    }

                    // Save new image
                    using (FileStream s = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(s);
                    }

                    originalPost.ImageURL = @"\images\posts\" + filename;
                }
                originalPost.Content = editedPost.Content;

                postRepository.Update(originalPost);
                await postRepository.SaveAsync();

                return RedirectToAction("Index");
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

            var currentLoggedUser = await GetCurrentLoggedUserAsync(useSignInManager: true);

            if (currentLoggedUser == null)
            {
                return Unauthorized();
            }

            var post = await postRepository.GetPostByIdAsync(postId);

            if (post == null)
            {
                return NotFound("Post not found.");
            }

            var like = await likeRepository.GetByOwnerAndPostAsync(currentLoggedUser.Id, postId);

            if (like == null)
            {
                like = new Like(postId, currentLoggedUser.Id);
                await likeRepository.AddAsync(like);
                post.Likes++;
            }
            else
            {
                likeRepository.Remove(like);
                post.Likes--;
            }

            postRepository.Update(post);
            await postRepository.SaveAsync();

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private async Task<ApplicationUser?> GetCurrentLoggedUserAsync(bool useSignInManager = true)
        {
            ApplicationUser? currentUser = useSignInManager
                ? await signInManager.UserManager.GetUserAsync(User)
                : await userManager.GetUserAsync(User);

            return currentUser;
        }

        private async Task<PostViewModel> GetViewModel()
        {
            var viewModel = new PostViewModel
            {
                Post = new Post(),
                Posts = await postRepository.GetAllAsync(),
                Comment = new Comment(),
                Comments = await commentRepository.GetAllAsync(),
                Like = new Like(),
                Likes = await likeRepository.GetAllAsync()
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
        private string TimeAgo(DateTime postedDate)
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
    }
}
