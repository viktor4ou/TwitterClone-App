using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Models.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public string? ImageURL { get; set; }
        public int Likes { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
