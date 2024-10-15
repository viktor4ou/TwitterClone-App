using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.Data;
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

        private readonly ApplicationDbContext _db;
        public ProfileController(UserManager<IdentityUser> userManager, CustomUserManager customUserManager, ApplicationDbContext db,SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _customUserManager = customUserManager;
            _db = db;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index(string userId)
        {
            var currentLoggedUser = _signInManager.UserManager.GetUserAsync(User).Result as ApplicationUser;
            if (currentLoggedUser is null)
            {
                return Unauthorized();
            }

            ProfileViewModel profileVM = GetProfileViewModel(userId);
            return View(profileVM);
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
                profileVM.Followers = _db.Followers.ToList();
                profileVM.Posts = _db.Posts.Where(i => i.PostOwnerId == user.Id).ToList();
            }
            return profileVM;
        }
    }
}
