using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SocialMedia.Models.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Comment should be between 1 and 255 symbols")]
        public string Content { get; set; } = string.Empty;

        [ValidateNever]
        public DateTime DatePosted { get; set; }

        [ValidateNever]
        public int PostId { get; set; }
        [NotMapped]
        [ValidateNever]
        public string TimeAgo { get; set; } = string.Empty;
        [ValidateNever]
        public string CommentOwnerId { get; set; }
    }
}