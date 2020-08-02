using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP4_Forum.Models.ViewModels
{
    public class SectionEditModel
    {
        public Section Section { get; set; }
        public IEnumerable<Partition> Members { get; set; }
        public IEnumerable<Partition> NonMembers { get; set; }
    }
}