using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP4_Forum.Models.ViewModels
{
    public class PartitionEditModel
    {
        public Partition Partition { get; set; }
        public IEnumerable<Theme> Members { get; set; }
        public IEnumerable<Theme> NonMembers { get; set; }
    }
}