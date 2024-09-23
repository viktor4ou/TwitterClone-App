using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Content should be between 1 and 255 symbols")]
        public string Content { get; set; }

        public string? ImageURL { get; set; }

        [ValidateNever]
        public int Likes { get; set; }

        [ValidateNever]
        public DateTime DatePosted { get; set; }

        [NotMapped]
        [ValidateNever]
        public string TimeAgo { get; set; }
    }
}