﻿using System;
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
        public Post Post { get; set; }
        [ValidateNever]
        public Comment Comment { get; set; }
        public List<Post> Posts = new();
        public List<Comment> Comments = new();
    }
}