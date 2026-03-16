using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.Interfaces;
using SocialMedia.Models.Models;
using SocialMedia_App.Controllers;

namespace SocialMedia_App.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(UserManager<ApplicationUser> userManager,
            CustomUserManager customUserManager,
            SignInManager<ApplicationUser> signInManager,
            IFollowerRepository followerRepository,
            IPostRepository postRepository)
            : base(userManager, customUserManager, signInManager, followerRepository, postRepository)
        {
        }
    }
}
