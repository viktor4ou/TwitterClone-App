using Microsoft.AspNetCore.Mvc;

namespace SocialMedia_App.Areas.User.Controllers
{
    public class ProfileController : Controller
    {
        [Area("User")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
