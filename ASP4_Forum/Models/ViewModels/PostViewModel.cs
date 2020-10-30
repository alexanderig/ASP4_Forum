﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP4_Forum.Models.ViewModels
{
    public class PostViewModel
    {
        public int ThemeID { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string PostText { get; set; }
        public string UserName { get; set; }
    }
}