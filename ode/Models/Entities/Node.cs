using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ode.Models.Entities
{
    public enum NodeType
    {
        File, Folder
    }

    /// <summary>
    /// TODO
    /// </summary>
    public class Node
    {
        /// <summary>
        /// The database-generated unique ID of the Node.
        /// </summary>
        public int ID { get; set; }

        public string Name { get; set; }

        public int ParentNodeID { get; set; } // Parent folder

        public int ProjectID { get; set; }

        [MaxLength(450)]
        public string CreatedByUserID { get; set; } // CreatedBy

        public NodeType Type { get; set; }

        //TODO MimeTypeID ...
        public DateTime CreatedDate { get; set; }
    }
}
