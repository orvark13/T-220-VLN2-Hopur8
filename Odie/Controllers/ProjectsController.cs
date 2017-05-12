using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Odie.Models.Entities;
using Odie.Models.ViewModels;
using Odie.Data;
using Odie.Services;
using Microsoft.AspNetCore.Identity;

namespace Odie.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private ProjectsService _projectsService;
        private FilesService _filesService;
        private UsersService _usersService;

        public ProjectsController(ProjectsService projectsService, FilesService filesService, UsersService usersService, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _projectsService = projectsService;
            _filesService = filesService;
            _usersService = usersService;
        }

        /// <summary>
        /// Generate a page with a list of the projects the current user has access to.
        /// </summary>
        /// <parameter name="msg">Numer of a standard message to show in the view.</parameter>
        /// <parameter name="hl">Id of project whose listing should be highlighted in the view.</parameter>
        public IActionResult Index(int? msg, int? hl)
        {
            var currentUserID = _userManager.GetUserId(User);

            var viewModel = new ProjectsPageViewModel()
            {
                Projects = _projectsService.GetUsersProjects(currentUserID),
                Templates = _projectsService.GetUsersTemplates(currentUserID),
                CurrentUser = _usersService.GetUserByID(currentUserID),
                Notice = new NoticeViewModel()
            };

            if (msg != null) {
                viewModel.Notice.MessageID = msg ?? 0;

                switch (msg)
                {
                    case -1:
                        viewModel.Notice.Message = "Failed to create new file.";
                        break;
                    case 1:
                        viewModel.Notice.Message = "New file has been added.";
                        break;
                    case 2:
                        viewModel.Notice.Message = "File has been deleted.";
                        break;
                }

                viewModel.Notice.NewID = hl ?? -1;
            }
            
            return View(viewModel);
        }

        /// <summary>
        /// POST to give create a project (or template) and give it the provided name.
        /// </summary>
        [HttpPost]
        public IActionResult Create(string name, bool template = false)
        {
            var currentUserID = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "New Project";
            }
            var projectId = _projectsService.Create(name, currentUserID);

            var projectID = _filesService.Create("README.md", projectId, currentUserID, "# README\n\nWrite something nice here :)");

            if (projectID > 0)
            {
                return RedirectToAction(nameof(ProjectsController.Index), "Projects", new { msg = 1, newID = projectID});
            }
            else
            {
                return RedirectToAction(nameof(ProjectsController.Index), "Projects", new { msg = -1});
            }
        }

        [HttpPost]
        /// <summary>
        /// POST to give the provided name to the project with given id. Reply in JSON indicates success or error, and reason.
        /// </summary>
        public IActionResult Rename(int id, string name)
        {
            if (_projectsService.UpdateName(id, name))
            {
                return Json(new { success = true});
            }
            else
            {
                return Json(new { success = false, error = "Rename failed."});
            }
        }

        /// <summary>
        /// Update the list of users that have acccess to the provided project ID so it matches the POSTed string with comma seperated userIDs.
        /// </summary>
        [HttpPost]
        public IActionResult Sharing(int id, string sharing)
        {
            var userIDs = new List<string>();

            Console.WriteLine("sharing = " + sharing);

            if (sharing != null)
            {
                string[] userIDList = sharing.Split(new char[] { ',' });
                foreach (var userID in userIDList)
                {
                    userIDs.Add(userID.Trim());
                }
            }
            
            Console.WriteLine(userIDs.ToString());

            if (_projectsService.UpdateSharing(id, userIDs))
            {
                return Json(new { success = true});
            }
            else
            {
                return Json(new { success = false, error = "Failed to update project sharing."});
            }
        }

        /// <summary>
        /// Reply to GET with a list in JSON of users (id, text) whose names (partially) match the given query term.
        /// </summary>
        [HttpGet]
        public JsonResult MatchingUsers(string term)
        {
            var users = _usersService.GetUsersMatchingName(term);

            var r = new {
                items = users
            };

            return Json(r);
        }

        /// <summary>
        /// Delete project with given ID and redirect to Projects page.
        /// </summary>
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            if (_projectsService.HasAccessToProject(_userManager.GetUserId(User), id ?? -1)) {
                _projectsService.DeleteProjectByID(id ?? -1);
                return RedirectToAction(nameof(ProjectsController.Index), "Projects", new { msg = 2});
            }
            else
            {
                return new ChallengeResult();
            }
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }
    }
}
