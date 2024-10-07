using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Models.Models
{
    public class Follower
    {
        [Key]
        public string FollowerId { get; set; }

        public string FollowOwnerId { get; set; }   
        public string FollowedUserId { get; set; }
    }
}
