using SocialMedia.Models.ViewModels;

namespace SocialMedia_App.Services.Interfaces
{
    public interface IChatService
    {
        Task<ChatViewModel> GetChatViewModelAsync(string userId);
    }
}
