﻿using Sparkle.Domain.Entities;
using System.Collections.Generic;

namespace Sparkle.Models
{
    public class UserProfileViewModel
    {
        public User User { get; set; }
        public IEnumerable<Post> Posts { get; set; }


    }
}
