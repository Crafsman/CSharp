﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class Post
    {
        public int ID { get; set; }
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";

        public DateTime Created { get; set; } = DateTime.Now;

    }
}
