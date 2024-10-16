using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.Data;
using SocialMedia.Models.Models;
using SocialMedia.Models.ViewModels;

namespace SocialMedia_App.Areas.User.Controllers
{
    [Area("User")]
    public class ChatController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly CustomUserManager customUserManager;
        private readonly ApplicationDbContext db;

        public ChatController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            CustomUserManager customUserManager,
            ApplicationDbContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.customUserManager = customUserManager;
            this.db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var currentLoggedUser = signInManager.UserManager.GetUserAsync(User).Result;
            if (currentLoggedUser is null)
            {
                TempData["ErrorMessage"] = "You need to log in to access this feature.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            ChatViewModel chatViewModel = GetChatViewModel(currentLoggedUser.Id);
            return View(chatViewModel);
        }

        private ChatViewModel GetChatViewModel(string userId)
        {
            //Add repository design pattern
            ChatViewModel chatViewModel = new();
            //it returns list of followers , get the followersId and then get the user details a
            List<Follower> followers = db.Followers.Where(f => f.FollowOwnerId == userId).ToList();
            List<string> userIds = followers.Select(f => f.FollowedUserId).ToList();
            chatViewModel.FollowedUsers = GetAllUsersById(userIds);

            return chatViewModel;
        }

        private List<ApplicationUser> GetAllUsersById(List<string> userIds)
        {
            List<ApplicationUser> users = new();
            foreach (var userId in userIds)
            {
                ApplicationUser user = customUserManager.FindByIdAsync(userId).Result as ApplicationUser;
                users.Add(user);
            }
            return users;
        }
    }
}
