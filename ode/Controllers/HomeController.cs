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

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _projectsService = new ProjectsService(_context);
        }

        public IActionResult Index()
        {
            string currentUserId = _userManager.GetUserId(User);
            Console.WriteLine(currentUserId);
            var viewModel = _projectsService.GetUsersProjects(currentUserId);
            return View(viewModel);
        }

/*public async Task<IActionResult> Index()
{
    return View(await _context.Projects.ToListAsync());
}*/

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }
    }
}
