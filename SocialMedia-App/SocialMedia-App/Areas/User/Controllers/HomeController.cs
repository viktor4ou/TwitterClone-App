using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.Interfaces;
using SocialMedia.Models.Models;
using SocialMedia.Models.ViewModels;
using System.Diagnostics;

namespace SocialMedia_App.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IPostRepository postRepository;
        private readonly ICommentRepository commentRepository;
        private readonly ILIkeRepository likeRepository;
        private readonly IUserRepository userRepository;

        public HomeController(
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
            var currentLoggedUser = await signInManager.UserManager.GetUserAsync(User);

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

                var currentLoggedUser = await signInManager.UserManager.GetUserAsync(User);

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
            var currentLoggedUser = await signInManager.UserManager.GetUserAsync(User);

            postVM.Comment.PostId = postId;
            postVM.Comment.DatePosted = DateTime.Now;
            postVM.Comment.CommentOwnerId = currentLoggedUser.Id;

            await commentRepository.AddAsync(postVM.Comment);
            await commentRepository.SaveAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeletePost(int id)
        {
            var user = await userManager.GetUserAsync(User);

            Post searchedPost = await postRepository.GetPostByIdAsync(id);

            if (searchedPost == null || searchedPost.PostOwnerId != user.Id)
            {
                return Unauthorized(); // Return 401 Unauthorized if the user is not the owner
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
            var currentLoggedUser = await signInManager.UserManager.GetUserAsync(User);

            Comment searchedComment = await commentRepository.GetByIdAsync(commentId);
            Post searchedPost = await postRepository.GetPostByIdAsync(postId);

            if ((searchedComment.CommentOwnerId != currentLoggedUser.Id) && currentLoggedUser.Id != searchedPost.PostOwnerId)
            {
                return Unauthorized();
            }

            commentRepository.Remove(searchedComment);
            await commentRepository.SaveAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditPost(int id)
        {
            var currentLoggedUser = await userManager.GetUserAsync(User);
            PostViewModel viewModel = await GetViewModel();
            viewModel.Post = await postRepository.GetPostByIdAsync(id);

            if (viewModel.Post == null || viewModel.Post.PostOwnerId != currentLoggedUser.Id)
            {
                return Unauthorized(); // Return 401 Unauthorized if the user is not the owner
            }

            return View(viewModel.Post);
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(Post editedPost, IFormFile? file)
        {
            var currentLoggedUser = await userManager.GetUserAsync(User);
            var originalPost = await postRepository.GetPostByIdAsync(editedPost.PostId);

            if (originalPost == null || originalPost.PostOwnerId != currentLoggedUser.Id)
            {
                return Unauthorized(); // Return 401 Unauthorized if the user is not the owner
            }

            if (ModelState.IsValid)
            {
                // Uploaded a new image
                if (file != null)
                {
                    string wwwRootPath = webHostEnvironment.WebRootPath;
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, "images", "posts");

                    // Delete existing image
                    if (editedPost.ImageURL != null)
                    {
                        string oldImagePath = Path.Combine(wwwRootPath, editedPost.ImageURL.TrimStart('\\'));
                        System.IO.File.Delete(oldImagePath);
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
            var currentLoggedUser = await signInManager.UserManager.GetUserAsync(User);

            var post = await postRepository.GetPostByIdAsync(postId);
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
