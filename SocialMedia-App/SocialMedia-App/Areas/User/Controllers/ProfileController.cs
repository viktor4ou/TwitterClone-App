using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models.Models;
using SocialMedia_App.Controllers;
using SocialMedia_App.Services.Interfaces;

namespace SocialMedia_App.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(
            SignInManager<ApplicationUser> signInManager,
            IProfileService profileService,
            ILogger<ProfileControllerBase> logger)
            : base(signInManager, profileService, logger)
        {
        }
    }
}
