using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.Models.Models;

namespace SocialMedia.Models.ViewModels
{
    public class PostViewModel
    {
        public PostViewModel()
        {
            
        }
        public Post Post { get; set; }
        public List<Post> Posts = new List<Post>();
    }
}
