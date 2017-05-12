using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Odie.Models.ViewModels
{
    /// <summary>
    /// Project data to be used in the UI view.
    /// </summary>
    public class ProjectViewModel
    {
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

    /// <summary>
    /// Holds a summary project data for use in lists in views.
    /// </summary>
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

    /// <summary>
    /// Holds all the neccessary for a single project to use in a view.
    /// </summary>
    public class ProjectsPageViewModel
    {
        public NoticeViewModel Notice { get; set; }
        public IEnumerable<ProjectListingViewModel> Projects { get; set; }
        public IEnumerable<ProjectListingViewModel> Templates { get; set; }
        public UserViewModel CurrentUser { get; set; }
    }
}
