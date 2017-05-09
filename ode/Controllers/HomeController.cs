﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ode.Models; // ApplicationUser
using ode.Data;
using ode.Services;
using Microsoft.AspNetCore.Identity;

namespace ode.Controllers
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

        public IActionResult Index()
        {
            var currentUserID = _userManager.GetUserId(User);

            var viewModel = new ProjectsPageViewModel()
            {
                Projects = _projectsService.GetUsersProjects(currentUserID),
                Templates = _projectsService.GetUsersTemplates(currentUserID),
                CurrentUser = _usersService.GetUserByID(currentUserID)
            };
            
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Post(int? id, string name, bool template = false)
        {
            if (id == null)
            {
                // /Projects/Post => new project
                var currentUserID = _userManager.GetUserId(User);
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = "New Project";
                }
                var projectId = _projectsService.Create(name, currentUserID);

                _filesService.Create("README.md", projectId, currentUserID, "# README\n\nDescribe you project here.");

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                // /Project/Post/ID
                // Only know how to update name
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = "#" + id.ToString();
                }
                _projectsService.UpdateName(id ?? -1, name);

                return new EmptyResult();
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

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            if (_projectsService.HasAccessToProject(_userManager.GetUserId(User), id ?? -1)) {
                _projectsService.DeleteProjectByID(id ?? -1);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                return new ChallengeResult();
            }
        }

        /*[HttpGet]
        public IEnumerable<UserSelectItemViewModel> GetUsers(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var items = new List<SelectItem>();

            string[] idList = id.Split(new char[] { ',' });
            foreach (var idStr in idList)
            {
                int idInt;
                if (int.TryParse(idStr, out idInt))
                {
                    items.Add(_makes.FirstOrDefault(m => m.id == idInt));
                }
            }

            return items;
        }*/

        [HttpGet]
        public ProjectViewModel GetSharing(int id)
        {
            return _projectsService.GetProjectByID(id);
        }

        [HttpPost]
        public ProjectViewModel PutSharing(int id)
        {
            return _projectsService.GetProjectByID(id);
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }
    }
}
