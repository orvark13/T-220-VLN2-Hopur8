using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Odie.Models.Entities;
using Odie.Models.ViewModels;
using Odie.Data;

namespace Odie.Services
{
    public class ProjectsService
    {
        private readonly ApplicationDbContext _context;
        private readonly UsersService _usersService;
        private readonly FilesService _filesService;

        public ProjectsService(ApplicationDbContext context, UsersService usersService, FilesService filesService)
        {
            _context = context;
            _usersService = usersService;
            _filesService = filesService;
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

        public bool HasAccessToProject(string userID, int projectID)
        {
            return _context.Projects
                .SingleOrDefault(p => p.ID == projectID && p.CreatedByUserID == userID) != null
                                || SharedWithUsers(projectID).Any(x => x.ID == userID);
        }

        public bool HasAccessToFile(string userID, int fileID)
        {
            return HasAccessToProject(userID, _filesService.GetFilesProjectID(fileID));
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

        public bool UpdateSharing(int projectID, List<string> updatedIDs)
        {
            // NOTE, these Linq Any subqueries produce "evaluated locally"
            // warnings. This is a known issue with .NET Core and EF.

            var currentSharings = _context.Sharings
                .Where(s => s.ProjectID == projectID);
            
            var removeSharings = currentSharings
                .Where(s => !updatedIDs.Any(s2 => s2 == s.UserID));

            var addSharings = updatedIDs
                .Where(i => !currentSharings.Any(s => s.UserID == i))
                .Select(i => new Sharing {
                    ProjectID = projectID,
                    UserID = i
                });

            // FIXME, make sure theese user ID that are being added are actual users.
            
            Console.WriteLine("Removing " + removeSharings.Count().ToString());
            Console.WriteLine("Adding " + addSharings.Count().ToString());

            if (removeSharings.Count() > 0 || addSharings.Count() > 0)
            {
                if (removeSharings.Count() > 0)
                {
                    _context.Sharings
                        .RemoveRange(removeSharings);
                }

                if (addSharings.Count() > 0)
                {
                    _context.Sharings
                        .AddRange(addSharings);
                }

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

            // Projects others are sharing with you.
            var othersProjects = _context.Projects
                .Join(_context.Sharings, p => p.ID, s => s.ProjectID, (p, s) => new ProjectSharingJoin(){ P = p, S = s})
                .Where(n => n.P.CreatedByUserID != applicationUserID
                            && n.S.UserID == applicationUserID
                            && n.P.Template == templates)
                .Select(x => new ProjectListingViewModel
                {
                    ID = x.P.ID,
                    Name = x.P.Name,
                    CreatedByUserID = x.P.CreatedByUserID,
                    Template = x.P.Template,
                    CreatedDate = x.P.CreatedDate
                })
                .ToList();

            foreach (var p in projects)
            {
                p.SharedWith = SharedWithUsers(p.ID);
                p.CreatedByUser = _usersService.GetUserByID(p.CreatedByUserID);
            }

            foreach (var p in othersProjects)
            {
                p.SharedWith = SharedWithUsers(p.ID);
                p.CreatedByUser = _usersService.GetUserByID(p.CreatedByUserID);

                projects.Add(p);
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

            _filesService.DeleteFilesByProjectID(projectID);
        }

        public ProjectViewModel GetProjectByID(int projectID)
        {
            var project = _context.Projects.SingleOrDefault(p => p.ID == projectID && p.Template == false);
            if (project == null)
            {
                return null;
            }

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

            foreach (FileViewModel f in files)
            {
                f.CreatedByUser = _usersService.GetUserByID(f.CreatedByUserID);
            }

            var mainFile = files
                .SingleOrDefault(n => n.ID == project.MainNodeID);

            var createdByUser = _usersService.GetUserByID(project.CreatedByUserID);

            var sharedWith = SharedWithUsers(projectID);

            var viewModel = new ProjectViewModel
            {
                ID = project.ID,
                Name = project.Name,

                MainNodeID = project.MainNodeID,
                MainFile = mainFile,
                CreatedByUserID = project.CreatedByUserID,
                CreatedByUser = createdByUser,
                CreatedDate = project.CreatedDate,

                SharedWith = sharedWith,
                Files = files
            };

            return viewModel;
        }
    }
}