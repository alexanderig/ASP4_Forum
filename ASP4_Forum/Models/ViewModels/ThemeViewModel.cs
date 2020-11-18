using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP4_Forum.Models.ViewModels
{
    public class ThemeViewModel
    {
        [Required]
        public int PartitionID { get; set; }
        public virtual Partition Partition { get; set; }
        [Required(ErrorMessage = "Theme must be not empty")]
        [Remote(action: "CheckTheme", controller: "Home", ErrorMessage = "Theme Name already in use")]
        [Display(Name = "Theme Name")]
        public string Name { get; set; }

    }
}