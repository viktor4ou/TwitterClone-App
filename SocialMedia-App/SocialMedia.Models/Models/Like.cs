using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SocialMedia.Models.Models
{
    public class Like
    {
        
        [Key]
        public int LikeId { get; set; }
        [ValidateNever]
        public int PostId { get; set; }
        [ValidateNever]
        public string LikeOwnerId { get; set; }
    }
}
