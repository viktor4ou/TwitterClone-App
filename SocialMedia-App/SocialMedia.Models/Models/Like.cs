using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Models.Models
{
    public class Like
    {
        public Like()
        {

        }
        public Like(int postId, string likeOwnerId)
        {
            LikeId = Guid.NewGuid().ToString();
            PostId = postId;
            LikeOwnerId = likeOwnerId;
        }

        

        [Key]
        [Required]
        public string LikeId { get; set; }

        [Required]
        public int PostId { get; set; }
        [Required]
        public string LikeOwnerId { get; set; }
    }
}
