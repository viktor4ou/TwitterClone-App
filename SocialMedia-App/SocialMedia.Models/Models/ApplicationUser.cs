using Microsoft.AspNetCore.Identity;

namespace SocialMedia.Models.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? ProfileImageURL { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Followers { get; set; }
        public int Following { get; set; }
        public string Discriminator { get; set; } = nameof(ApplicationUser);
    }
}
