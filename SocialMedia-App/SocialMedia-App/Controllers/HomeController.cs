using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SocialMedia.Data.Data;
using SocialMedia.Models.Models;

namespace SocialMedia_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;
        public HomeController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Post post)// TODO: rename to CreatePost ,add image functionality 
        {
            post.DatePosted = DateTime.Now;
            db.Posts.Add(post);
            db.SaveChanges();
            return View();
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
    }
}
