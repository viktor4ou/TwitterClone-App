using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SocialMedia.Data.Data;
using SocialMedia.Models.Models;
using Microsoft.AspNetCore.Hosting;
using SocialMedia.Models.ViewModels;

namespace SocialMedia_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;

        public HomeController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {

            this.db = db;
            this.webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            PostViewModel viewModel = GetViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Index(PostViewModel postVM, IFormFile? file)// TODO: rename to CreatePost ,add image functionality 
        {
            //For some reason the comment needs to be valid 
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

                postVM.Post.DatePosted = DateTime.Now;
                db.Posts.Add(postVM.Post);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
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
        public IActionResult Privacy()
        {
            return View();
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
