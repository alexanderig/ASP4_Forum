using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace ASP4_Forum.Models.ViewModels
{
    public class ThemeEditModel
    {
        public Theme Theme { get; set; }
        public int Posts { get; set; } 
        //public IEnumerable<Post> Members { get; set; }
        //public IEnumerable<Post> NonMembers { get; set; }

    }
}