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
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly CustomUserManager _customUserManager;
        private readonly IFollowerRepository followerRepository;
        private readonly IPostRepository postRepository;

        public ProfileController(UserManager<ApplicationUser> userManager,
            CustomUserManager customUserManager,
            SignInManager<ApplicationUser> signInManager,
            IFollowerRepository followerRepository,
            IPostRepository postRepository)
        {
            this.postRepository = postRepository;
            this.followerRepository = followerRepository;
            _userManager = userManager;
            _customUserManager = customUserManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string userId)
        {
            var searchedUser = await _userManager.FindByIdAsync(userId);
            if (searchedUser is null)
            {
                return BadRequest();
            }
            ProfileViewModel profileVM = await GetProfileViewModel(userId);
            return View(profileVM);
        }

        [HttpPost]
        public async Task<IActionResult> Follow(string userId)
        {
            var currentLoggedUser = await _signInManager.UserManager.GetUserAsync(User);
            var userToFollow = await _userManager.FindByIdAsync(userId);

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

        [HttpPost]
        public async Task<IActionResult> Unfollow(string userId)
        {
            var currentLoggedUser = await _signInManager.UserManager.GetUserAsync(User);

            var followToBeRemoved = followerRepository.GetBy(f =>
                f.FollowOwnerId == currentLoggedUser.Id &&
                f.FollowedUserId == userId);

            var userToUnfollow = await _userManager.FindByIdAsync(userId);

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
        private async Task<ProfileViewModel> GetProfileViewModel(string userId)
        {
            ProfileViewModel profileVM = new();
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                profileVM.UserID = user.Id;
                profileVM.Username = user.UserName;
                profileVM.FristName = await _customUserManager.GetFirstNameAsync(user);
                profileVM.LastName = await _customUserManager.GetLastNameAsync(user);
                profileVM.ProfilePictureURL = await _customUserManager.GetImageURLAsync(user);
                profileVM.FollowersCount = await _customUserManager.GetFollowersCountAsync(user);
                profileVM.Following = await _customUserManager.GetFollowingCountAsync(user);
                profileVM.Followers = followerRepository.GetAll();
                profileVM.Posts = postRepository.GetAllBy(i => i.PostOwnerId == user.Id);
            }
            return profileVM;
        }
    }
}
