using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models.Models;

namespace SocialMedia_App.Areas.User.Controllers
{
    [Area("User")]
    public class ChatController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly CustomUserManager customUserManager;
        public ChatController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            CustomUserManager customUserManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.customUserManager = customUserManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
