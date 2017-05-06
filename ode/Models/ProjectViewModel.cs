using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ode.Models
{
    public class ProjectViewModel
    {
        //TODO
        public int ID { get; set; }
        public string Name { get; set; }
        public int MainNodeID { get; set; }
        public string CreatedByUserID { get; set; } // CreatedBy
        //public IEnumerable<ApplicationUser> SharedWith { get; set; }
        public bool Template { get; set; }
        public DateTime CreatedDate { get; set; }

        public IEnumerable<FileViewModel> Files { get; set; }
    }
}
