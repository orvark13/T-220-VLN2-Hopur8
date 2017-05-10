using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ode.Models
{
    public class ProjectViewModel
    {
        //TODO
        public int ID { get; set; }
        public string Name { get; set; }
        public int MainNodeID { get; set; }
        public FileViewModel MainFile { get; set; }
        public string CreatedByUserID { get; set; }
        public UserViewModel CreatedByUser { get; set; }
        public bool Template { get; set; }
        [DisplayFormat(DataFormatString = "{0:M/d}")]
        public DateTime CreatedDate { get; set; }

        public IEnumerable<UserViewModel> SharedWith { get; set; }
        public IEnumerable<FileViewModel> Files { get; set; }
    }

    public class ProjectListingViewModel
    {
        //TODO
        public int ID { get; set; }
        public string Name { get; set; }
        public string CreatedByUserID { get; set; }
        public UserViewModel CreatedByUser { get; set; }
        public bool Template { get; set; }
        [DisplayFormat(DataFormatString = "{0:M/d}")]
        public DateTime CreatedDate { get; set; }

        public IEnumerable<UserViewModel> SharedWith { get; set; }
    }

    public class ProjectsPageViewModel
    {
        // TODO
        public NoticeViewModel Notice { get; set; }
        public IEnumerable<ProjectListingViewModel> Projects { get; set; }
        public IEnumerable<ProjectListingViewModel> Templates { get; set; }
        public UserViewModel CurrentUser { get; set; }
    }
}
