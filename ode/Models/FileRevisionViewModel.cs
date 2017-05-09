using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ode.Models;

namespace ode.Models
{
    public class FileRevisionViewModel
    {
        /// <summary>
        /// TODO
        /// </summary>
        public int ID { get; set; }

        public int NodeID { get; set; }

        public string Description { get; set; }

        public string Contents { get; set; }

        public string CreatedByUserID { get; set; } // CreatedBy

        public DateTime CreatedDate { get; set; }
    }
}