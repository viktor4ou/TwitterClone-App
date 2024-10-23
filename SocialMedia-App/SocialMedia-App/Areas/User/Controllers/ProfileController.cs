using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.Data;
using SocialMedia.Data.Repository;
using SocialMedia.Models.Models;
using SocialMedia.Models.ViewModels;

namespace SocialMedia_App.Areas.User.Controllers
{
    [Area("User")]
    public class ProfileController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly CustomUserManager _customUserManager;
        private readonly FollowerRepository followerRepository;
        private readonly PostRepository postRepository;

        public ProfileController(UserManager<IdentityUser> userManager,
            CustomUserManager customUserManager,
            SignInManager<IdentityUser> signInManager,
            FollowerRepository followerRepository,
            PostRepository postRepository)
        {
            this.postRepository = postRepository;
            this.followerRepository = followerRepository;
            _userManager = userManager;
            _customUserManager = customUserManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index(string userId)
        {
            var currentLoggedUser = _signInManager.UserManager.GetUserAsync(User).Result as ApplicationUser;
            if (currentLoggedUser is null)
            {
                TempData["ErrorMessage"] = "You need to log in to access this feature.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var searchedUser = _userManager.FindByIdAsync(userId).Result as ApplicationUser;
            if (searchedUser is null)
            {
                return BadRequest();
            }
            ProfileViewModel profileVM = GetProfileViewModel(userId);
            return View(profileVM);
        }
        [HttpPost]
        public IActionResult Follow(string userId)
        {
            var currentLoggedUser = _signInManager.UserManager.GetUserAsync(User).Result as ApplicationUser;

            if (currentLoggedUser is null)
            {
                TempData["ErrorMessage"] = "You need to log in to access this feature.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var userToFollow = _userManager.FindByIdAsync(userId).Result as ApplicationUser;
            if (userToFollow != null)
            {
                Follower follower = new()
                {
                    FollowerId = Guid.NewGuid().ToString(),
                    FollowOwnerId = currentLoggedUser.Id,
                    FollowedUserId = userToFollow.Id
                };

                userToFollow.Followers++;
                currentLoggedUser.Following++;
                followerRepository.Add(follower);
                followerRepository.Save();

                return RedirectToAction("Index", new { userId = userId });
            }

            return Unauthorized();
        }

        public IActionResult Unfollow(string userId)
        {
            var currentLoggedUser = _signInManager.UserManager.GetUserAsync(User).Result as ApplicationUser;
            if (currentLoggedUser is null)
            {
                TempData["ErrorMessage"] = "You need to log in to access this feature.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var followToBeRemoved = followerRepository.GetBy(f =>
                f.FollowOwnerId == currentLoggedUser.Id &&
                f.FollowedUserId == userId);
            var userToUnfollow = _userManager.FindByIdAsync(userId).Result as ApplicationUser;
            if (followToBeRemoved is null)
            {
                return BadRequest();
            }
            currentLoggedUser.Following--;
            userToUnfollow.Followers--;
            followerRepository.Remove(followToBeRemoved);
            followerRepository.Save();
            return RedirectToAction("Index", new { userId = userId });
        }
        private ProfileViewModel GetProfileViewModel(string userId)
        {
            ProfileViewModel profileVM = new();
            var user = _userManager.FindByIdAsync(userId).Result as ApplicationUser;
            if (user != null)
            {
                profileVM.UserID = user.Id;
                profileVM.Username = user.UserName;
                profileVM.FristName = _customUserManager.GetFirstNameAsync(user).Result;
                profileVM.LastName = _customUserManager.GetLastNameAsync(user).Result;
                profileVM.ProfilePictureURL = _customUserManager.GetImageURLAsync(user).Result;
                profileVM.FollowersCount = _customUserManager.GetFollowersCountAsync(user).Result;
                profileVM.Following = _customUserManager.GetFollowingCountAsync(user).Result;
                profileVM.Followers = followerRepository.GetAll()
                profileVM.Posts = postRepository.GetAllBy(i => i.PostOwnerId == user.Id);
            }
            return profileVM;
        }
    }
}
