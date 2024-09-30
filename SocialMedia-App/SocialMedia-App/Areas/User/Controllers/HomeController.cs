using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SocialMedia.Data.Data;
using SocialMedia.Models.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using SocialMedia.Models.ViewModels;

namespace SocialMedia_App.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public HomeController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {

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

        [HttpPost]
        public IActionResult CreatePost(PostViewModel postVM, IFormFile? file)// TODO: rename to CreatePost ,add image functionality 
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string wwwRootPath = webHostEnvironment.WebRootPath;
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\posts");

                    using (FileStream s = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(s);
                    }

                    postVM.Post.ImageURL = @"\images\posts\" + filename;
                }

                var user = signInManager.UserManager.GetUserAsync(User).Result;
                postVM.Post.PostOwnerId = user.Id;
                postVM.Post.DatePosted = DateTime.Now;
                db.Posts.Add(postVM.Post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["ShowModal"] = true;
            return View("Index", postVM);
        }


        [HttpPost]
        public IActionResult CreateComment(int postId, PostViewModel postVM)
        {
            postVM.Comment.PostId = postId;
            postVM.Comment.DatePosted = DateTime.Now;
            db.Comments.Add(postVM.Comment);
            db.SaveChanges();
            return RedirectToAction("Index");
            //How to make it to just load the comment without refreshing the page like in a real time chat
        }

        public IActionResult DeletePost(int id)
        {
            Post searchedPost = db.Posts.Find(id);
            List<Comment> postComments = db.Comments.Where(c => c.PostId == id).ToList();
            //Delete existing image
            if (searchedPost.ImageURL != null)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;
                string imageFullPath = Path.Combine(wwwRootPath, searchedPost.ImageURL.TrimStart('\\'));
                System.IO.File.Delete(imageFullPath);
            }
            db.Posts.Remove(searchedPost);
            db.Comments.RemoveRange(postComments);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult DeleteComment(int id)
        {
            Comment searchedComment = db.Comments.Find(id);
            db.Comments.Remove(searchedComment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult EditPost(int id)
        {
            PostViewModel viewModel = GetViewModel();
            viewModel.Post = db.Posts.Find(id);
            return View(viewModel.Post);
        }
        [HttpPost]
        public IActionResult EditPost(Post editedPost, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                //Uploaded an new image
                if (file != null)
                {
                    string wwwRootPath = webHostEnvironment.WebRootPath;
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\posts");
                    //Delete existing image
                    if (editedPost.ImageURL != null)
                    {
                        string oldImagePath = Path.Combine(wwwRootPath, editedPost.ImageURL.TrimStart('\\'));
                        System.IO.File.Delete(oldImagePath);
                    }
                    //Save new image
                    using (FileStream s = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(s);
                    }

                    editedPost.ImageURL = @"\images\posts\" + filename;
                }

                editedPost.DatePosted = DateTime.Now;
                db.Posts.Update(editedPost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("EditPost", editedPost);
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
                Posts = db.Posts.ToList(),
                Comment = new Comment(),
                Comments = db.Comments.ToList()
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
