using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Odie.Models;
using Odie.Models.Entities;
using Odie.Data;
using Microsoft.AspNetCore.Identity;

namespace Odie.Services
{
    // <summary>
    // UserService add a couple of user searching methods to ApplicationUser.
    // </summary>
    public class UsersService
    {
        private readonly ApplicationDbContext _context;

        public UsersService(ApplicationDbContext context)
        {
            _context = context;
        }

        // <summary>
        // Get a list of users whose names match a search string.
        // </summary>
        // <parameter name="partial">User name search query</parameter>
        // <returns>A list of zero or more users.</returns>
        public List<UserSelectItemViewModel> GetUsersMatchingName(string partial)
        {
            var users = _context.Users.Where(m => m.UserName.ToLower().Contains(partial.ToLower()))
                .Select(x => new UserSelectItemViewModel
                {
                    ID = x.Id,
                    Text = x.UserName
                })
                .Take(10)
                .ToList();

            return users;
        }

        // <summary>
        // Get a user view model for the user with the specified ID.
        // </summary>
        // <parameter name="userID">A user ID.</parameter>
        // <returns></returns>
        public UserViewModel GetUserByID(string userID)
        {
            var applicationUser = _context.Users
                    .SingleOrDefault(u => u.Id == userID);

            var user = new UserViewModel
            {
                ID = applicationUser.Id,
                Name = applicationUser.UserName,
                Email = applicationUser.Email
            };

            return user;
        }
    }
}