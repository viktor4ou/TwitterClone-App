using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SocialMedia.Data.Data;
using SocialMedia.Models.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using SocialMedia.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Interfaces;
using SocialMedia.Data.Repository;

namespace SocialMedia_App.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IPostRepository postRepository;
        private readonly ICommentRepository commentRepository;
        public HomeController(ApplicationDbContext db,
            IWebHostEnvironment webHostEnvironment,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IPostRepository postRepository,
            ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
            this.postRepository = postRepository;
            this.db = db;
            this.webHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        public IActionResult Index()
        {
            PostViewModel viewModel = GetViewModel();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            var currentLoggedUser = signInManager.UserManager.GetUserAsync(User).Result;
            if (currentLoggedUser is null)
            {
                TempData["ErrorMessage"] = "You need to log in to access this feature.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new List<object>()); // Return an empty list if the query is empty
            }

            // Select both UserName and Id to return to the client
            var users = await db.ApplicationUsers
                .Where(u => u.UserName.Contains(query))
                .Select(u => new { u.UserName, u.Id,u.ProfileImageURL }) // Select both UserName and Id
                .Take(10)
                .ToListAsync();

            return Json(users); // Return the list of users as JSON
        }

        [HttpPost]
        public IActionResult CreatePost(PostViewModel postVM, IFormFile? file)
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

                var currentLoggedUser = signInManager.UserManager.GetUserAsync(User).Result;

                if (currentLoggedUser is null)
                {
                    TempData["ErrorMessage"] = "You need to log in to access this feature.";
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }

                postVM.Post.PostOwnerId = currentLoggedUser.Id;
                postVM.Post.DatePosted = DateTime.Now;

                postRepository.Add(postVM.Post);
                postRepository.Save();

                return RedirectToAction("Index");
            }
            ViewData["ShowModal"] = true;

            return View("Index", postVM);
        }

        [HttpPost]
        public IActionResult CreateComment(int postId, PostViewModel postVM)
        {
            var currentLoggedUser = signInManager.UserManager.GetUserAsync(User).Result;
            if (currentLoggedUser is null)
            {
                TempData["ErrorMessage"] = "You need to log in to access this feature.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            postVM.Comment.PostId = postId;
            postVM.Comment.DatePosted = DateTime.Now;
            postVM.Comment.CommentOwnerId = currentLoggedUser.Id;

            db.Comments.Add(postVM.Comment);
            db.SaveChanges();

            return RedirectToAction("Index");
            //How to make it to just load the comment without refreshing the page like in react
        }

        public IActionResult DeletePost(int id)
        {
            var user = userManager.GetUserAsync(User).Result;
            if (user is null)
            {
                TempData["ErrorMessage"] = "You need to log in to access this feature.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            Post searchedPost = postRepository.GetPostById(id);

            if (searchedPost == null || searchedPost.PostOwnerId != user.Id)
            {
                return Unauthorized(); // Return 401 Unauthorized if the user is not the owner
            }

            List<Comment> postComments = db.Comments.Where(c => c.PostId == id).ToList();

            // Delete existing image
            if (searchedPost.ImageURL != null)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;
                string imageFullPath = Path.Combine(wwwRootPath, searchedPost.ImageURL.TrimStart('\\'));
                System.IO.File.Delete(imageFullPath);
            }

            postRepository.Remove(searchedPost);
            commentRepository.RemoveRange(postComments);
            postRepository.Save();

            return RedirectToAction("Index");
        }

        public IActionResult DeleteComment(int commentId, int postId)
        {
            var currentLoggedUser = signInManager.UserManager.GetUserAsync(User).Result;
            if (currentLoggedUser is null)
            {
                TempData["ErrorMessage"] = "You need to log in to access this feature.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            Comment searchedComment = db.Comments.Find(commentId);
            Post searchedPost = postRepository.GetPostById(postId);

            if ((searchedComment.CommentOwnerId != currentLoggedUser.Id) && currentLoggedUser.Id != searchedPost.PostOwnerId)
            {
                return Unauthorized();
            }

            db.Comments.Remove(searchedComment);
            postRepository.Save();

            return RedirectToAction("Index");
        }

        public IActionResult EditPost(int id)
        {
            var currentLoggedUser = userManager.GetUserAsync(User).Result;
            if (currentLoggedUser is null)
            {
                TempData["ErrorMessage"] = "You need to log in to access this feature.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            PostViewModel viewModel = GetViewModel();
            viewModel.Post = postRepository.GetPostById(id);

            if (viewModel.Post == null || viewModel.Post.PostOwnerId != currentLoggedUser.Id)
            {
                return Unauthorized(); // Return 401 Unauthorized if the user is not the owner
            }

            return View(viewModel.Post);
        }

        [HttpPost]
        public IActionResult EditPost(Post editedPost, IFormFile? file)
        {
            var currentLoggedUser = userManager.GetUserAsync(User).Result;
            if (currentLoggedUser is null)
            {
                TempData["ErrorMessage"] = "You need to log in to access this feature.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            //TODO: Add into repo Func for search
            var originalPost = db.Posts.FirstOrDefault(p => p.PostId == editedPost.PostId);

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

                    editedPost.ImageURL = @"\images\posts\" + filename;
                }

                editedPost.DatePosted = DateTime.Now;
                editedPost.PostOwnerId = currentLoggedUser.Id;

                postRepository.Update(editedPost);
                postRepository.Save();

                return RedirectToAction("Index");
            }
            return View("EditPost", editedPost);
        }
        [HttpPost]
        public IActionResult LikePost(int postId)
        {
            var currentLoggedUser = signInManager.UserManager.GetUserAsync(User).Result;
            if (currentLoggedUser is null)
            {
                TempData["ErrorMessage"] = "You need to log in to access this feature.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var post = postRepository.GetPostById(postId);
            var like = db.Likes.FirstOrDefault(i => i.LikeOwnerId == currentLoggedUser.Id && i.PostId == postId);

            if (like == null)
            {
                like = new Like(postId, currentLoggedUser.Id);
                db.Likes.Add(like);
                post.Likes++;
            }
            else
            {
                db.Likes.Remove(like);
                post.Likes--;
            }

            postRepository.Update(post);
            postRepository.Save();

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private PostViewModel GetViewModel()
        {
            var viewModel = new PostViewModel
            {
                Post = new Post(),
                Posts = postRepository.GetAll(),
                Comment = new Comment(),
                Comments = db.Comments.ToList(),
                Like = new Like(),
                Likes = db.Likes.ToList()
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
