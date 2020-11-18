using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP4_Forum.Models
{
    public class Theme
    {
        public Theme()
        {
            Posts = new HashSet<Post>();
        }
        public int ID { get; set; }
        [Required(ErrorMessage = "Theme cannot be empty")]
        //[RegularExpression(@"[\w\s]+", ErrorMessage = "Incorrect Theme Name")]
        [Remote(action: "CheckTheme", controller: "Home", ErrorMessage = "Theme Name already in use")]
        [Display(Name = "Theme Name")]
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public virtual ApplicationUser Creator { get; set; }
        public virtual ApplicationUser Curator { get; set; }
        public virtual Partition Partition { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }

}