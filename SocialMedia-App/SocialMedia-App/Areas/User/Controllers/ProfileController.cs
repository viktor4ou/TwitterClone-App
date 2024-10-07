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
        private readonly CustomUserManager _customUserManager;
        private readonly ApplicationDbContext _db;
        public ProfileController(UserManager<IdentityUser> userManager, CustomUserManager customUserManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _customUserManager = customUserManager;
            _db = db;
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
            if (user != null)
            {
                profileVM.UserID = user.Id;
                profileVM.Username = user.UserName;
                profileVM.FristName = _customUserManager.GetFirstNameAsync(user as ApplicationUser).Result;
                profileVM.LastName = _customUserManager.GetLastNameAsync(user as ApplicationUser).Result;
                profileVM.ProfilePictureURL = _customUserManager.GetImageURLAsync(user as ApplicationUser).Result;
                profileVM.FollowersCount = _customUserManager.GetFollowersCountAsync(user as ApplicationUser).Result;
                profileVM.Following = _customUserManager.GetFollowingCountAsync(user as ApplicationUser).Result;
                profileVM.Posts = _db.Posts.Where(i => i.PostOwnerId == user.Id).ToList();
            }
            return profileVM;
        }
    }
}
