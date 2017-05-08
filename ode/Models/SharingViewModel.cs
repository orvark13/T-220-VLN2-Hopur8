using System;
using System.Collections.Generic;

namespace ode.Models
{
    public class SharingViewModel
    {
        //TODO
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public int MainNodeID { get; set; }
        public FileViewModel MainFile { get; set; }
        public string CreatedByUserID { get; set; }
        public UserViewModel CreatedByUser { get; set; }
        public bool Template { get; set; }
        public DateTime CreatedDate { get; set; }

        public IEnumerable<UserViewModel> SharedWith { get; set; }
        public IEnumerable<FileViewModel> Files { get; set; }
    }
}