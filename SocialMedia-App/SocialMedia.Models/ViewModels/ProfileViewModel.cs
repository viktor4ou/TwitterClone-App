using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Models.ViewModels
{
    public class ProfileViewModel
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public string FristName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureURL { get; set; }
        public int Followers { get; set; }
        public int Following { get; set; }
    }
}
