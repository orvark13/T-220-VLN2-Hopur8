using Xunit;
using System;
using Odie.Services;
using Odie.Data;
using Odie.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Odie.Tests.Services
{
    public class ProjectsService_GetProjectByIDShould
    {
        private readonly UsersService _usersService;
        private readonly FilesService _filesService;
        private readonly ProjectsService _projectsService;

        public ProjectsService_GetProjectByIDShould()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ProjectsService_GetProjectByIDShould")
                .Options;

            var context = new ApplicationDbContext(options);
            _usersService = new UsersService(context);
            _filesService = new FilesService(context, _usersService);
            _projectsService = new ProjectsService(context, _usersService, _filesService);

            // Seed the the fake Database.
            DbInitializer.Initialize(context);
        }

        [Fact]
        public void ReturnExistingProject()
        {
            var users = _usersService.GetUsersMatchingName("orvark13@ru.is");

            int newProjectID = _projectsService.Create("Test", users[0].ID);

            var p = _projectsService.GetProjectByID(newProjectID);

            var result = p != null && p.ID == newProjectID && p.Name == "Test" && p.Template == false;

            Assert.True(result, $"Found an existing project by ID");
        }

        [Fact]
        public void ReturnEmptyModelForNonExistingProject()
        {
            var result = _projectsService.GetProjectByID(-1);

            Assert.True(result == null, $"No project exists with ID -1");
        }

    }
}