using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ode.Models;
using ode.Models.Entities;

namespace ode.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Sharing> Sharings { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<FileRevision> FileRevisions { get; set; }
        public DbSet<Node> Nodes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Sharing>()
                .HasKey(s => new { s.ProjectID, s.UserID });

            builder.Entity<Sharing>()
                .ToTable("ProjectSharing");
            builder.Entity<Project>();
            builder.Entity<FileRevision>();
            builder.Entity<Node>();
        }
    }
}
