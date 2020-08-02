using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP4_Forum.Models
{
    public class City
    {
        public City()
        {
            Users = new HashSet<ApplicationUser>();
        }
        public int ID { get; set; }
        public string CityName { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}