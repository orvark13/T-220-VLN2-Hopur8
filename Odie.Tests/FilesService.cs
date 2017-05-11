using Xunit;
using System;
using Odie.Services;
using Odie.Data;
using Odie.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Odie.Tests.Services
{
    public class FilesService_IsNameUniqueShould
    {
        private readonly UsersService _usersService;
        private readonly FilesService _filesService;

        public FilesService_IsNameUniqueShould()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "FilesService_IsNameUniqueShould")
                .Options;

            var context = new ApplicationDbContext(options);
            _usersService = new UsersService(context);
            _filesService = new FilesService(context, _usersService);

            // Seed the the fake Database.
            DbInitializer.Initialize(context);
        }

        [Fact]
        public void NotAcceptExistingFileNames()
        {
            var result = _filesService.IsNameUnique(1, "README.md");

            Assert.False(result, $"README.md should not be accepted");
        }

        [Fact]
        public void AcceptNonExistingFileNames()
        {
            var result = _filesService.IsNameUnique(1, "Bingo.md");

            Assert.True(result, $"Bingo.md should be accepted");
        }
    }
}