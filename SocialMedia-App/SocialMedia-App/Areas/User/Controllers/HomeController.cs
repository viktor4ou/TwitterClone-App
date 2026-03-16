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
    public class HomeController : HomeControllerBase
    {
        public HomeController(
            IWebHostEnvironment webHostEnvironment,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            ILIkeRepository likeRepository,
            IUserRepository userRepository)
            : base(webHostEnvironment, userManager, signInManager, postRepository, commentRepository, likeRepository, userRepository)
        {

        }
    }
}
