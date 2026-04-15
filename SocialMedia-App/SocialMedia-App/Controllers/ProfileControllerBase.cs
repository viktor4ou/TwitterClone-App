using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models.Models;
using SocialMedia_App.Services;
using SocialMedia_App.Services.Interfaces;

namespace SocialMedia_App.Controllers
{
    public abstract class ProfileControllerBase : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IProfileService _profileService;
        private readonly ILogger<ProfileControllerBase> _logger;

        protected ProfileControllerBase(
            SignInManager<ApplicationUser> signInManager,
            IProfileService profileService,
            ILogger<ProfileControllerBase> logger)
        {
            _signInManager = signInManager;
            _profileService = profileService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string userId)
        {
            var profileVM = await _profileService.GetProfileViewModelAsync(userId);
            if (profileVM == null) return BadRequest();
            return View(profileVM);
        }

        [HttpPost]
        public async Task<IActionResult> Follow(string userId)
        {
            var currentUser = await _signInManager.UserManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            var success = await _profileService.FollowAsync(currentUser.Id, userId);
            if (!success) return Unauthorized();

            return RedirectToAction("Index", new { userId });
        }

        [HttpPost]
        public async Task<IActionResult> Unfollow(string userId)
        {
            var currentUser = await _signInManager.UserManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            var result = await _profileService.UnfollowAsync(currentUser.Id, userId);
            if (result == OperationResult.BadRequest) return BadRequest();

            return RedirectToAction("Index", new { userId });
        }
    }
}
