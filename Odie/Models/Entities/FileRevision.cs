using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Odie.Models.Entities
{
    /// <summary>
    /// Revisions editions of the contents of a given file node.
    /// </summary>
    public class FileRevision
    {
        /// <summary>
        /// The database-generated unique ID of the File Revision.
        /// </summary>
        public int ID { get; set; }

        public int NodeID { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "varbinary(max)")]
        public byte[] Contents { get; set; }

        [MaxLength(450)]
        public string CreatedByUserID { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
