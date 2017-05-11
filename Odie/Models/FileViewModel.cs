using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Odie.Models;

namespace Odie.Models
{
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

    public class Lambdas
    {
        Func<Entities.Node, FileViewModel> NodeToFileViewModel = x => new FileViewModel
        {
            ID = x.ID,
            Name = x.Name,
            ParentNodeID = x.ParentNodeID,
            ProjectID = x.ProjectID,
            CreatedByUserID = x.CreatedByUserID,
            CreatedDate = x.CreatedDate
        };
    }
}