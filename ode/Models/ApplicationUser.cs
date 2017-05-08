using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ode.Models.Entities;

// TODO create a ViewModel version of ApplicationUser (ID, Name, email)

namespace ode.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        //public virtual ICollection<Project> Projects { get; set; }
    }
}
