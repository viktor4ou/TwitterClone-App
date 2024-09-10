using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SocialMedia.Models.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        [Required]
        public string Content { get; set; }
        [ValidateNever]
        public DateTime DatePosted { get; set; }
        [ValidateNever]
        public int PostId { get; set; }
        [ForeignKey("PostId")]
        [ValidateNever]
        public Post Post { get; set; }

        [NotMapped]
        [ValidateNever]
        public string TimeAgo { get; set; }
    }
}
