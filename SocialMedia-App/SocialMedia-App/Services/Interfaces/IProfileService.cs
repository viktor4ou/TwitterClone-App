using SocialMedia.Models.ViewModels;

namespace SocialMedia_App.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileViewModel?> GetProfileViewModelAsync(string userId);
        Task<bool> FollowAsync(string currentUserId, string targetUserId);
        Task<OperationResult> UnfollowAsync(string currentUserId, string targetUserId);
    }
}
