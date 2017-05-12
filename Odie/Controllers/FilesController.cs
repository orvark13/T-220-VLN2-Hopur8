using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Odie.Models;
using Odie.Models.Entities;
using Odie.Models.ViewModels;
using Odie.Data;
using Odie.Services;
using Microsoft.AspNetCore.Identity;

namespace Odie.Controllers
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

        public IActionResult Index(int? id, int? msg, int? hl)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var project = _projectsService.GetProjectByID(id ?? -1);

            if (project == null)
            {
                return NotFound();
            }

            var viewModel = new FilesPageViewModel() {
                Project = project,
                Notice = new NoticeViewModel()
            };

            if (msg != null) {
                viewModel.Notice.MessageID = msg ?? 0;

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

            if (_projectsService.HasAccessToProject(_userManager.GetUserId(User), id ?? -1)) {
                return View(viewModel);
            }
            else
            {
                return new ChallengeResult();
            }
        }

        [HttpPost]
        public IActionResult Create(int id, string name, string open)
        {
            // /Files/Create/pID => new file in project
            var currentUserID = _userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(name)
                || !_filesService.IsNameUnique(id, name))
            {
                name = "NN.md";
            }
            var nodeID = _filesService.Create(name, id, currentUserID, "# " + name + "\n\n");

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
        /// <summary>
        /// Post method to rename a file node.
        /// </summary>
        /// <parameter name="id">File node ID.</parameter>
        /// <parameter name="name">Files new name.</parameter>
        public IActionResult Rename(int id, string name)
        {
            if (_projectsService.HasAccessToFile(_userManager.GetUserId(User), id))
            {
                var projectID = _filesService.GetFilesProjectID(id);

                if (_filesService.IsNameUnique(projectID, name))
                {
                    if (_filesService.UpdateName(id, name))
                    {
                        return Json(new { success = true});
                    }
                    else
                    {
                        return Json(new { success = false, error = "Rename failed."});
                    }
                }
                else
                {
                    return Json(new { success = false, error = "Rename failed! An existing project file already has that name."});
                }
            }
            else
            {
                return Json(new { success = false, error = "Access denied!"});
            }
        }

        [HttpPost]
        /// <summary>
        /// Post method to validate a name for a possible new file node in a project.
        /// </summary>
        /// <parameter name="id">Project ID.</parameter>
        /// <parameter name="name">Files new name.</parameter>
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

        /// <summary>
        /// Post method to delete a file node.
        /// </summary>
        /// <parameter name="id">File node ID.</parameter>
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

        /// <summary>
        /// Editor page.
        /// </summary>
        /// <parameter name="id">File node ID.</parameter>
        /// <parameter name="rev">File revision ID.</parameter>
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

        /// <summary>
        /// Update the contents of a file revision.
        /// </summary>
        /// <parameter name="nodeID">File node ID.</parameter>
        /// <parameter name="fileRevisionID">File revision ID.</parameter>
        /// <parameter name="contents">New revision contents.</parameter>
        public void SaveRevision(int nodeID, int fileRevisionID, string contents)
        {
            // FIXME: Only updating the current revision for now.
            // Will be changed when implementation of revision functionaly is finished.
            if (_projectsService.HasAccessToFile(_userManager.GetUserId(User), nodeID)) {
                _filesService.UpdateFileRevision(fileRevisionID, contents);
            }
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }
    }
}
