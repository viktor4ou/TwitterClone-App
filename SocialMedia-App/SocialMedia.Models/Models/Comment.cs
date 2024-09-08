using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Models.Models
{
    public class Comment
    {
        [ForeignKey("PostId")]
        public int PostId { get; set; }
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }    
    }
}
