using System;

namespace Odie.Models.ViewModels
{
    /// <summary>
    /// Holds a revision version of af file node.
    /// </summary>
    public class FileRevisionViewModel
    {
        public int ID { get; set; }

        public int NodeID { get; set; }

        public string Description { get; set; }

        public string Contents { get; set; }

        public string CreatedByUserID { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}