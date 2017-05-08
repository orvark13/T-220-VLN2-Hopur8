using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ode.Models.Entities
{
    /// <summary>
    /// TODO
    /// </summary>
    public class Project
    {
        /// <summary>
        /// The database-generated unique ID of the Project.
        /// </summary>
        public int ID { get; set; }

        public string Name { get; set; }

        public int MainNodeID { get; set; } // Mainfile

        public bool Template { get; set; }

        [MaxLength(450)]
        public string CreatedByUserID { get; set; } // CreatedBy

        //public ICollection<ApplicationUser> SharedWith { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }
}
