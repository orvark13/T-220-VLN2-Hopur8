using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ode.Models;
using ode.Models.Entities; // NodeType
using ode.Data;

namespace ode.Services
{
    public class ProjectsService
    {
        private readonly ApplicationDbContext _context;

        public ProjectsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ProjectViewModel> GetUsersProjects(string applicationUserID)
        {
            var projects = _context.Projects
                .Where(n => n.CreatedByUserID == applicationUserID && n.Template == false)
                .Select(x => new ProjectViewModel
                {
                    ID = x.ID,
                    Name = x.Name,
                    MainNodeID = x.MainNodeID, // TODO
                    CreatedByUserID = x.CreatedByUserID, // TODO
                    //SharedWith = x.SharedWith,
                    Template = x.Template,
                    CreatedDate = x.CreatedDate,
                    Files = null // TODO
                })
                .ToList();

            return projects;
        }

        /*public ProjectViewModel GetProjectByID(int projectID)
        {
            var project = _context.Projects.SingleOrDefault(p => p.ID == projectID);
            if (project == null)
            {
                // TODO error
            }
            // TODO confirm project not template ?

            var files = _context.Nodes
                .Where(n => n.ProjectID == projectID && n.Type == NodeType.File)
                .Select(x => new FileViewModel
                {
                    ID = x.ID,
                    Name = x.Name

                })
                .ToList();

            // TODO fetch MainfileID Node
            // TODO fetch ApplicationUser list from SharedWith
            // TODO fetch OwnedBy

            var viewModel = new ProjectViewModel
            {
                ID = project.ID,
                Name = project.Name,

                MainfileID = project.MainfileID,
                ApplicationUserID = project.ApplicationUserID,  // CreatedBy
                // SharedWith
                // Template
                CreatedDate = project.CreatedDate//,

                //Files = files
            };

            return viewModel;
        }*/
    }
}