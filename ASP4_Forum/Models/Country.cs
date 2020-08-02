using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP4_Forum.Models
{
    public class Country
    {
        public Country()
        {
            Users = new HashSet<ApplicationUser>();
            Cities = new HashSet<City>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }
}