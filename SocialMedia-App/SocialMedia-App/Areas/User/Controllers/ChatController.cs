using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models.Models;
using SocialMedia_App.Services.Interfaces;

namespace SocialMedia_App.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class ChatController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IChatService _chatService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(
            SignInManager<ApplicationUser> signInManager,
            IChatService chatService,
            ILogger<ChatController> logger)
        {
            _signInManager = signInManager;
            _chatService = chatService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _signInManager.UserManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            _logger.LogInformation("User {UserId} opened chat", currentUser.Id);
            var chatViewModel = await _chatService.GetChatViewModelAsync(currentUser.Id);
            return View(chatViewModel);
        }
    }
}
