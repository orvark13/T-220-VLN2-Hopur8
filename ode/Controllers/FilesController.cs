using System;
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
    public class FilesController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private ProjectsService _projectsService;
        private FilesService _filesService;

        public FilesController(ProjectsService projectsService, FilesService filesService, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _projectsService = projectsService;
            _filesService = filesService;
        }

        public IActionResult Index(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var viewModel = _projectsService.GetProjectByID(id ?? -1);

            if (_projectsService.HasAccessToProject(_userManager.GetUserId(User), id ?? -1)) {
                return View(viewModel);
            }
            else
            {
                return new ChallengeResult();
            }
        }

        [HttpPost]
        public IActionResult Post(int id, string name)
        {
            // /Files/Post/pID => new file in project
            var currentUserID = _userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(name))
            {
                name = "NN.md";
            }
            _filesService.Create(name, id, currentUserID, "");

            return RedirectToAction(nameof(FilesController.Index), "Files", new {id = id});
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            if (_projectsService.HasAccessToFile(_userManager.GetUserId(User), id ?? -1)) {
                var projectID = _filesService.GetFilesProjectID(id ?? -1);
                _filesService.DeleteFileByID(id ?? -1);
                return RedirectToAction(nameof(FilesController.Index), "Files", new {id = projectID});
            }
            else
            {
                return new ChallengeResult();
            }
        }

        public IActionResult Editor(int? id, int? rev)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var viewModel = new EditorViewModel() {
                File = _filesService.GetFile(id ?? -1),
                FileRevision = _filesService.GetFileRevision(id ?? -1, rev)
            };

            if (_projectsService.HasAccessToFile(_userManager.GetUserId(User), id ?? -1)) {
                return View(viewModel);
            }
            else
            {
                return new ChallengeResult();
            }
        }

        public void SaveRevision(int nodeID, int fileRevisionID, string contents)
        {
            // Only updating the current revision for now.
            // Will be changed when implementation of revision functionaly is finished.
            _filesService.UpdateFileRevision(nodeID, fileRevisionID, contents);
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }
    }
}
