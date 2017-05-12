using System;

namespace Odie.Models.ViewModels
{
    /// <summary>
    /// Holds data for the file nodes for use in views.
    /// </summary>
    public class FileViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ParentNodeID { get; set; } // ParentID
        public int ProjectID { get; set; }
        public string CreatedByUserID { get; set; } // CreatedBy
        public UserViewModel CreatedByUser {get; set; }
        public DateTime CreatedDate { get; set; }
    }
}