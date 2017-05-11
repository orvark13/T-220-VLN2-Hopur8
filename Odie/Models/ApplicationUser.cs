using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Odie.Models.Entities;

// TODO create a ViewModel version of ApplicationUser (ID, Name, email)

namespace Odie.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        //public virtual ICollection<Project> Projects { get; set; }
    }
}
