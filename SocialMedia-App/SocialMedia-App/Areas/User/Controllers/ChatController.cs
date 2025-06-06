using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.Interfaces;
using SocialMedia.Models.Models;
using SocialMedia.Models.ViewModels;

namespace SocialMedia_App.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
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
        public async Task<IActionResult> Index()
        {
            var currentLoggedUser = await signInManager.UserManager.GetUserAsync(User);

            ChatViewModel chatViewModel = await GetChatViewModel(currentLoggedUser.Id);
            return View(chatViewModel);
        }

        private async Task<ChatViewModel> GetChatViewModel(string userId)
        {
            ChatViewModel chatViewModel = new();
            //it returns list of followers , get the followersId and then get the user details 
            List<Follower> followers = await followerRepository.GetFollowersByUserIdAsync(userId);
            List<string> userIds = followers.Select(f => f.FollowedUserId).ToList();
            chatViewModel.FollowedUsers = await GetAllUsersById(userIds);

            return chatViewModel;
        }

        private async Task<List<ApplicationUser>> GetAllUsersById(List<string> userIds)
        {
            List<ApplicationUser> users = new();
            foreach (var userId in userIds)
            {
                ApplicationUser user = await customUserManager.FindByIdAsync(userId);
                users.Add(user);
            }
            return users;
        }
    }
}
