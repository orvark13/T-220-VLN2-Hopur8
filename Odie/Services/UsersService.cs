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
    public class UsersService
    {
        //private UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UsersService(ApplicationDbContext context/*, UserManager<ApplicationUser> userManager*/)
        {
            //_userManager = userManager;
            _context = context;
        }

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