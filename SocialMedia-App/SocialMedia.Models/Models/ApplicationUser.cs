using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace SocialMedia.Models.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string?  ProfileImageURL { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
