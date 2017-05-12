using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Odie.Models; // ApplicationUser
using Odie.Data;
using Odie.Services;
using Microsoft.AspNetCore.Identity;

namespace Odie.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private ProjectsService _projectsService;
        private FilesService _filesService;
        private UsersService _usersService;

        public HomeController(ProjectsService projectsService, FilesService filesService, UsersService usersService, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _projectsService = projectsService;
            _filesService = filesService;
            _usersService = usersService;
        }

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
                viewModel.Notice = new NoticeViewModel() {
                    MessageID = msg ?? 0
                };

                switch (msg)
                {
                    case -1:
                        viewModel.Notice.Message = "Failed to create new project.";
                        break;
                    case 1:
                        viewModel.Notice.Message = "New project successfully created.";
                        break;
                    case 2:
                        viewModel.Notice.Message = "Project has been deleted.";
                        break;
                }

                viewModel.Notice.NewID = hl ?? -1;
            }
            
            return View(viewModel);
        }

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
                return RedirectToAction(nameof(HomeController.Index), "Home", new { msg = 1, newID = projectID});
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home", new { msg = -1});
            }
        }

        [HttpPost]
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

        [HttpGet]
        public JsonResult MatchingUsers(string term)
        {
            var users = _usersService.GetUsersMatchingName(term);

            var r = new {
                items = users
            };

            return Json(r);
        }

        // <summary>
        // 
        // </summary>
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            if (_projectsService.HasAccessToProject(_userManager.GetUserId(User), id ?? -1)) {
                _projectsService.DeleteProjectByID(id ?? -1);
                return RedirectToAction(nameof(HomeController.Index), "Home", new { msg = 2});
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
