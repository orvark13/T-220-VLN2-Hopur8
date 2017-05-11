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
    public class UsersService_GetUsersMatchingNameShould
    {
        private readonly UsersService _usersService;

        public UsersService_GetUsersMatchingNameShould()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "UsersService_GetUsersMatchingNameShould")
                .Options;

            var context = new ApplicationDbContext(options);
            _usersService = new UsersService(context);

            // Seed the the fake Database.
            DbInitializer.Initialize(context);
        }

        [Fact]
        public void FindUserGivenFullName()
        {
            var result = _usersService.GetUsersMatchingName("orvark13@ru.is");

            Assert.True(result.Count > 0 && result[0].Text == "orvark13@ru.is", $"orvark13@ru.is should be found");
        }
    }
}

