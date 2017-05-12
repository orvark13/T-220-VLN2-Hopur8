using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Odie.Models.Entities
{
    public enum NodeType
    {
        File, Folder
    }

    /// <summary>
    /// File node metadata.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// The database-generated unique ID of the Node.
        /// </summary>
        public int ID { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// The node ID of the parent folder.
        /// </summary>
        public int ParentNodeID { get; set; }

        public int ProjectID { get; set; }

        [MaxLength(450)]
        public string CreatedByUserID { get; set; } // CreatedBy

        public NodeType Type { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
