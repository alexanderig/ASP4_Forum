using ASP4_Forum.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP4_Forum.Models
{
    public class Partition
    {
        public Partition()
        {
            Themes = new HashSet<Theme>();
        }
        public int ID { get; set; }
        [Required(ErrorMessage = "Partition must be not empty")]
        //[RegularExpression(@"[\w\s]+", ErrorMessage = "Incorrect Partition Name")]
        [Remote(action: "CheckPartition", controller: "Home", ErrorMessage = "Partition Name already in use")]
        [Display(Name = "Partition Name")]
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public virtual Section Section { get; set; }
        public virtual ICollection<Theme> Themes { get; set; }
    }
}