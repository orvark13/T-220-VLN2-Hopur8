using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ode.Models;
using ode.Models.Entities;
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

        public int Create(string name, string userID)
        {
            var project = new Project()
            {
                Name = name,
                Template = false,
                CreatedByUserID = userID,
                CreatedDate = DateTime.Now
            };

            _context.Projects.Add(project);
            _context.SaveChanges();

            return project.ID;
        }

        public bool UpdateName(int projectID, string name)
        {
            if (name.Length > 0)
            {
                var project = _context.Projects
                    .Where(p => p.ID == projectID)
                    .SingleOrDefault();
                
                project.Name = name;

                _context.SaveChanges();

                return true;
            }
            return false;
        }

        public List<ProjectListingViewModel> GetUsersProjects(string applicationUserID, bool templates = false)
        {
            var projects = _context.Projects
                .Where(n => n.CreatedByUserID == applicationUserID && n.Template == templates)
                .Select(x => new ProjectListingViewModel
                {
                    ID = x.ID,
                    Name = x.Name,
                    CreatedByUserID = x.CreatedByUserID,
                    Template = x.Template,
                    CreatedDate = x.CreatedDate
                })
                .ToList();
            
            // FIXME also projects shared with you!

            foreach (ProjectListingViewModel p in projects)
            {
                p.SharedWith = SharedWithUsers(p.ID);
                p.CreatedByUser = GetUserByID(p.CreatedByUserID);
            }

            return projects;
        }

        public List<ProjectListingViewModel> GetUsersTemplates(string applicationUserID)
        {
            return GetUsersProjects(applicationUserID, true);
        }

        public List<UserViewModel> SharedWithUsers(int projectID)
        {
            var sharedWithUserIDs = _context.Sharings
                .Where(s => s.ProjectID == projectID)
                .Select(x => x.UserID);

            var sharedWithUsers = _context.Users
                .Where(u => sharedWithUserIDs.Contains(u.Id))
                .Select(x => new UserViewModel
                {
                    ID = x.Id,
                    Name = x.UserName,
                    Email = x.Email
                })
                .ToList();

            return sharedWithUsers;            
        }

        public List<UserSelectItemViewModel> GetUsersMatchingName(string partial)
        {
            var users = _context.Users.Where(m => m.UserName.ToLower().Contains(partial.ToLower()))
                .Select(x => new UserSelectItemViewModel
                {
                    ID = x.Id,
                    Text = x.UserName
                })
                .Take(10)
                .ToList();

            return users;            
        }

        public UserViewModel GetUserByID(string userID)
        {
            var applicationUser = _context.Users
                .SingleOrDefault(u => u.Id == userID);
            
            var user = new UserViewModel
            {
                ID = applicationUser.Id,
                Name = applicationUser.UserName,
                Email = applicationUser.Email
            };

            return user;
        }

        public void DeleteProjectByID(int projectID)
        {
            if (projectID < 0)
            {
                return;
            }

            var project = _context.Projects
                .SingleOrDefault(m => m.ID == projectID);

            if (project == null)
            {
                return;
            }

            _context.Projects.Remove(project);
            _context.SaveChanges();

            return;
        }

        public ProjectViewModel GetProjectByID(int projectID)
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
                    Name = x.Name,
                    ParentNodeID = x.ParentNodeID,
                    ProjectID = x.ProjectID,
                    CreatedByUserID = x.CreatedByUserID,
                    CreatedDate = x.CreatedDate
                })
                .ToList();

            var mainFile = files
                .SingleOrDefault(n => n.ID == project.MainNodeID);

            var createdByUser = GetUserByID(project.CreatedByUserID);

            var sharedWith = SharedWithUsers(projectID);

            var viewModel = new ProjectViewModel
            {
                ID = project.ID,
                Name = project.Name,

                MainNodeID = project.MainNodeID,
                MainFile = mainFile,
                CreatedByUserID = project.CreatedByUserID,
                CreatedByUser = createdByUser,
                // Template
                CreatedDate = project.CreatedDate,

                SharedWith = sharedWith,
                Files = files
            };

            return viewModel;
        }
    }
}