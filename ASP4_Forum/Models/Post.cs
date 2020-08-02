using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP4_Forum.Models
{
    public class Post
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public DateTime EditedDate { get; set; }
        public bool IsBanned { get; set; }
        public string Text { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public virtual Theme Theme { get; set; }

    }
}