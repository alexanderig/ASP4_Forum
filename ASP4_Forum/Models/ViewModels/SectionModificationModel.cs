using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP4_Forum.Models.ViewModels
{
    public class SectionModificationModel
    {
        [Required]
        public int ID { get; set; }
        public int[] IdsToAdd { get; set; }
        public int[] IDChild { get; set; }
        public int[] IDParent { get; set; }

    }

}