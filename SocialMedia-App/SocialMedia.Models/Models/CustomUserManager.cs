﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SocialMedia.Models.Models
{
    public class CustomUserManager : UserManager<IdentityUser>
    {
        public CustomUserManager(IUserStore<IdentityUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<IdentityUser> passwordHasher, IEnumerable<IUserValidator<IdentityUser>> userValidators, IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<IdentityUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public async Task<IdentityResult> SetFirstNameAsync(ApplicationUser user, string firstName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.FirstName = firstName;
            return await UpdateAsync(user);
        }
        public async Task<IdentityResult> SetLastNameAsync(ApplicationUser user, string lastName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.LastName = lastName;
            return await UpdateAsync(user);
        }

        public async Task<IdentityResult> SetProfileImageURLAsync(ApplicationUser user, string filename)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.ProfileImageURL = Path.Combine(@"\images\profile", filename);
            return await UpdateAsync(user);
        }
        public async Task<string> GetFirstNameAsync(ApplicationUser user)
        {
            return user.FirstName;
        }

        public async Task<int> GetFollowersCountAsync(ApplicationUser user)
        {
            return user.Followers;
        }

        public async Task<int> GetFollowingCountAsync(ApplicationUser user)
        {
            return user.Following;
        }

        public async Task<string> GetImageURLAsync(ApplicationUser user)
        {
            return user.ProfileImageURL;
        }
        public async Task<string> GetLastNameAsync(ApplicationUser user)
        {
            return user.LastName;
        }
    }
}
