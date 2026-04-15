using Microsoft.AspNetCore.Identity;
using SocialMedia.Data.Interfaces;
using SocialMedia.Models.Models;
using SocialMedia.Models.ViewModels;
using SocialMedia_App.Services.Interfaces;

namespace SocialMedia_App.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CustomUserManager _customUserManager;
        private readonly IFollowerRepository _followerRepository;
        private readonly IPostRepository _postRepository;
        private readonly ILogger<ProfileService> _logger;

        public ProfileService(
            UserManager<ApplicationUser> userManager,
            CustomUserManager customUserManager,
            IFollowerRepository followerRepository,
            IPostRepository postRepository,
            ILogger<ProfileService> logger)
        {
            _userManager = userManager;
            _customUserManager = customUserManager;
            _followerRepository = followerRepository;
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task<ProfileViewModel?> GetProfileViewModelAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Profile requested for non-existent user {UserId}", userId);
                return null;
            }

            var profileVM = new ProfileViewModel
            {
                UserID = user.Id,
                Username = user.UserName,
                FristName = await _customUserManager.GetFirstNameAsync(user),
                LastName = await _customUserManager.GetLastNameAsync(user),
                ProfilePictureURL = await _customUserManager.GetImageURLAsync(user),
                FollowersCount = await _customUserManager.GetFollowersCountAsync(user),
                Following = await _customUserManager.GetFollowingCountAsync(user),
                Followers = await _followerRepository.GetAllAsync(),
                Posts = await _postRepository.GetAllPostsByUserId(user.Id)
            };

            _logger.LogInformation("Profile page loaded for user {UserId}", userId);
            return profileVM;
        }

        public async Task<bool> FollowAsync(string currentUserId, string targetUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            var userToFollow = await _userManager.FindByIdAsync(targetUserId);

            if (currentUser == null || userToFollow == null)
            {
                _logger.LogWarning("Follow failed: user not found");
                return false;
            }

            var follower = new Follower
            {
                FollowerId = Guid.NewGuid().ToString(),
                FollowOwnerId = currentUserId,
                FollowedUserId = targetUserId
            };

            userToFollow.Followers++;
            currentUser.Following++;
            await _followerRepository.AddAsync(follower);
            await _followerRepository.SaveAsync();

            _logger.LogInformation("User {FollowerId} followed user {FollowedId}", currentUserId, targetUserId);
            return true;
        }

        public async Task<OperationResult> UnfollowAsync(string currentUserId, string targetUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            var userToUnfollow = await _userManager.FindByIdAsync(targetUserId);
            var followToBeRemoved = await _followerRepository.GetByOwnerAndFollowedAsync(currentUserId, targetUserId);

            if (followToBeRemoved == null)
            {
                _logger.LogWarning("Unfollow failed: no follow relationship between {FollowerId} and {FollowedId}", currentUserId, targetUserId);
                return OperationResult.BadRequest;
            }

            if (currentUser == null || userToUnfollow == null)
            {
                return OperationResult.NotFound;
            }

            currentUser.Following--;
            userToUnfollow.Followers--;
            _followerRepository.Remove(followToBeRemoved);
            await _followerRepository.SaveAsync();

            _logger.LogInformation("User {FollowerId} unfollowed user {FollowedId}", currentUserId, targetUserId);
            return OperationResult.Success;
        }
    }
}
