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
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private ProjectsService _projectsService;
        private string _currentUserID;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _projectsService = new ProjectsService(_context);
        }

        public IActionResult Index()
        {
            var currentUserID = _userManager.GetUserId(User);

            var viewModel = new ProjectsPageViewModel()
            {
                Projects = _projectsService.GetUsersProjects(currentUserID),
                Templates = _projectsService.GetUsersTemplates(currentUserID),
                CurrentUser = _projectsService.GetUserByID(currentUserID)
            };
            
            return View(viewModel);
        }

        [HttpPost]
        public bool Post(int? id, string name, bool template = false)
        {
            if (id == null)
            {
                // /Projects/Post => new project
                var currentUserID = _userManager.GetUserId(User);
                return _projectsService.Create(name, currentUserID);
            }
            else
            {
                // /Project/Post/ID
                // Only know how to update name
                return _projectsService.UpdateName(id ?? -1, name);
            }
        }

        [HttpGet]
        public JsonResult MatchingUsers(string term)
        {
            var users = _projectsService.GetUsersMatchingName(term);

            var r = new {
                items = users
            };

            return Json(r);
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

        public IActionResult Open(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            ViewData["Message"] = "Here be das Filelist!";

            var viewModel = _projectsService.GetProjectByID(id ?? -1);

            if (true) { // TODO check if currentUser is authorized ...
                return View(viewModel);
            }
            else
            {
                return new ChallengeResult();
            }
        }

        public IActionResult Editor(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var viewModel = new EditorViewModel() {};

            ViewData["Message"] = "Here be Editing";

            if (true) { // TODO check if currentUser is authorized ...
                return View(viewModel);
            }
            else
            {
                return new ChallengeResult();
            }
        }

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
