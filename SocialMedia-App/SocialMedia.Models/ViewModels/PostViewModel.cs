using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SocialMedia.Models.Models;

namespace SocialMedia.Models.ViewModels
{
    public class PostViewModel
    {
        public PostViewModel()
        {

        }
        [ValidateNever]
        public ApplicationUser User { get; set; }
        [ValidateNever]
        public Like Like { get; set; }  
        public Post Post { get; set; }
        [ValidateNever]
        public Comment Comment { get; set; }

        public List<Like> Likes = new();
        public List<Post> Posts = new();
        public List<Comment> Comments = new();
    }
}
