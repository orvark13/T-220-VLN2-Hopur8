using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ode.Models;

namespace ode.Models
{
    public class EditorViewModel
    {
        public FileViewModel File { get; set; }
        public FileRevisionViewModel FileRevision { get; set; }
    }
}