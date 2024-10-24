// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SocialMedia.Models.Models;

namespace SocialMedia_App.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly CustomUserManager _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public IndexModel(
            CustomUserManager userManager,
            SignInManager<IdentityUser> signInManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Username")]
            public string Username { get; set; }
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            [Display(Name = "Profile Image")]
            public IFormFile ProfileImage { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var username = await _userManager.GetUserNameAsync(user);
            var firstName = await _userManager.GetFirstNameAsync(user);
            var lastName = await _userManager.GetLastNameAsync(user);
            Email = email;
            Username = username;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Username = username,
                FirstName = firstName,
                LastName = lastName
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            //Not a good idea of using casting, because it breaks the abstraction of the UserManager
            var user = await _userManager.GetUserAsync(User) as ApplicationUser;
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //TODO:add change profile picture functionality
            var user = await _userManager.GetUserAsync(User) as ApplicationUser;
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            var username = await _userManager.GetUserNameAsync(user);
            if (Input.Username != username)
            {
                var setUsername = await _userManager.SetUserNameAsync(user, Input.Username);
                if (!setUsername.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set username.";
                    return RedirectToPage();
                }
            }

            var firstName = await _userManager.GetFirstNameAsync(user);
            if (Input.FirstName != firstName)
            {
                var setFirstName = await _userManager.SetFirstNameAsync(user, Input.FirstName);
                if (!setFirstName.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set first name.";
                    return RedirectToPage();
                }
            }

            var profileImage = await _userManager.GetImageURLAsync(user);
            if (Input.ProfileImage != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string productPath = Path.Combine(wwwRootPath, "images", "profile");
                string oldImagePath = Path.Combine(wwwRootPath, profileImage);
                //delete old image
                //System.IO.File.Delete(oldImagePath.TrimStart('\\'));

                //save the new image 
                string filename = Guid.NewGuid() + Path.GetExtension(Input.ProfileImage.FileName);

                using (FileStream stream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                {
                    Input.ProfileImage.CopyTo(stream);
                }
                await _userManager.SetProfileImageURLAsync(user, filename);
            }
            var lastName = await _userManager.GetLastNameAsync(user);
            if (Input.LastName != lastName)
            {
                var setLastName = await _userManager.SetLastNameAsync(user, Input.LastName);
                if (!setLastName.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set last name.";
                    return RedirectToPage();
                }
            }
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
