using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP4_Forum.Models.ViewModels
{
    public class PartitionViewModel
    {      
            [Required]
            public int SectionID { get; set; }
            public virtual Section Section { get; set; }
            [Required(ErrorMessage = "Partition must be not empty")]
            [Remote(action: "CheckPartition", controller: "Home", ErrorMessage = "Partition Name already in use")]
            [Display(Name = "Partition Name")]
            public string Name { get; set; }
            
    }

  

}