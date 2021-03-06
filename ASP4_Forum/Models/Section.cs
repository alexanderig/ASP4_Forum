﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP4_Forum.Models
{
    public class Section
    {
        public Section()
        {
            Partitions = new HashSet<Partition>();
        }

        public int ID { get; set; }

        [Required(ErrorMessage = "Section must be not empty")]
        [Remote(action: "CheckSection", controller: "Home", ErrorMessage = "Section Name already in use")]
        [Display(Name = "Section Name")]
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<Partition> Partitions { get; set; }
    }
}