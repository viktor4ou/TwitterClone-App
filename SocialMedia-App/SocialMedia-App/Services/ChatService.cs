using SocialMedia.Data.Interfaces;
using SocialMedia.Models.Models;
using SocialMedia.Models.ViewModels;
using SocialMedia_App.Services.Interfaces;

namespace SocialMedia_App.Services
{
    public class ChatService : IChatService
    {
        private readonly CustomUserManager _customUserManager;
        private readonly IFollowerRepository _followerRepository;
        private readonly ILogger<ChatService> _logger;

        public ChatService(
            CustomUserManager customUserManager,
            IFollowerRepository followerRepository,
            ILogger<ChatService> logger)
        {
            _customUserManager = customUserManager;
            _followerRepository = followerRepository;
            _logger = logger;
        }

        public async Task<ChatViewModel> GetChatViewModelAsync(string userId)
        {
            var chatViewModel = new ChatViewModel();
            List<Follower> followers = await _followerRepository.GetFollowersByUserIdAsync(userId);
            List<string> userIds = followers.Select(f => f.FollowedUserId).ToList();

            var users = new List<ApplicationUser>();
            foreach (var id in userIds)
            {
                ApplicationUser user = await _customUserManager.FindByIdAsync(id);
                if (user != null)
                {
                    users.Add(user);
                }
            }
            chatViewModel.FollowedUsers = users;

            _logger.LogInformation("Chat view model loaded for user {UserId}", userId);
            return chatViewModel;
        }
    }
}
