using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Odie.Models.Entities
{
    /// <summary>
    /// Project data.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// The database-generated unique ID of the Project.
        /// </summary>
        public int ID { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// The node ID of the main file in the project.
        /// </summary>
        public int MainNodeID { get; set; }

        public bool Template { get; set; }

        [MaxLength(450)]
        public string CreatedByUserID { get; set; }

        //public ICollection<ApplicationUser> SharedWith { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }
}
