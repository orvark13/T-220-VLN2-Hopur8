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
        public IActionResult Post(int id, string name, string open = "on")
        {
            // /Files/Post/pID => new file in project
            var currentUserID = _userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(name))
            {
                name = "NN.md";
            }
            var nodeID = _filesService.Create(name, id, currentUserID, "");

            if (open == "on" && nodeID > 0)
            {
                // If successfully created and user wanted to edit, then we open newly created file for editing.
                return RedirectToAction("Editor", "Files", new {id = nodeID});
            }
            else
            {
                // Back to project's file listing.
                if (nodeID > 0)
                {
                    return RedirectToAction(nameof(FilesController.Index), "Files", new {id = id, msg = 1, hl = nodeID});
                }
                else
                {
                    // Showing error.
                    return RedirectToAction(nameof(FilesController.Index), "Files", new {id = id, msg = -1});
                }
            }
        }

        [HttpPost]
        public IActionResult ValidateName(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Json(new { success = false, error = "Filename cannot be empty."});
            }

            if (_filesService.IsNameUnique(id, name))
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, error = "There is already a file with the same name."});
            }
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
            // FIXME: Only updating the current revision for now.
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
