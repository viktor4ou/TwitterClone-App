using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models.Models;
using SocialMedia.Models.ViewModels;

namespace SocialMedia_App.Areas.User.Controllers
{
    [Area("User")]
    public class ProfileController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly CustomUserManager _customUserManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public ProfileController(UserManager<IdentityUser> userManager, CustomUserManager customUserManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _customUserManager = customUserManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ProfileViewModel profileVM = GetProfileViewModel();
            return View(profileVM);
        }

        private ProfileViewModel GetProfileViewModel()
        {
            ProfileViewModel profileVM = new();
            var user = _userManager.GetUserAsync(User).Result;
            profileVM.UserID = user.Id;
            profileVM.Username = user.UserName;
            profileVM.FristName = _customUserManager.GetFirstNameAsync(user as ApplicationUser).Result;
            profileVM.LastName = _customUserManager.GetLastNameAsync(user as ApplicationUser).Result;
            profileVM.ProfilePictureURL = _customUserManager.GetImageURLAsync(user as ApplicationUser).Result;
            profileVM.Followers = _customUserManager.GetFollowersCountAsync(user as ApplicationUser).Result;
            profileVM.Following = _customUserManager.GetFollowingCountAsync(user as ApplicationUser).Result;
            return profileVM;
        }
    }
}
