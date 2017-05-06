using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ode.Models
{
    public class FileViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ParentNodeID { get; set; } // ParentID
        public int ProjectID { get; set; }
        public string CreatedByUserID { get; set; } // CreatedBy
        public DateTime CreatedDate { get; set; }
    }
}