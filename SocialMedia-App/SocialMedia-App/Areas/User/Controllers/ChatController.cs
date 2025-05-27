using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.Interfaces;
using SocialMedia.Models.Models;
using SocialMedia.Models.ViewModels;

namespace SocialMedia_App.Areas.User.Controllers
{
    [Area("User")]
    public class ChatController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly CustomUserManager customUserManager;
        private readonly IFollowerRepository followerRepository;

        public ChatController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            CustomUserManager customUserManager,
            IFollowerRepository followerRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.customUserManager = customUserManager;
            this.followerRepository = followerRepository;
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
            List<Follower> followers = followerRepository.GetAllBy(f => f.FollowOwnerId == userId);
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
