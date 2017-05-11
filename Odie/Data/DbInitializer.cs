using System;
using System.Linq;
using Odie.Models.Entities;
using Odie.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Odie.Data
{
    // <summary>
    // Initialize the database with test data.
    // </summary>
    public static class DbInitializer
    {
        // <summary>
        // Seeds the database with some initial data.
        // </summary>
        // <parameter name="context">The database context.</parameter>
        // <returns></returns>
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var userOrvar = new ApplicationUser {
                UserName = "orvark13@ru.is",
                NormalizedUserName = "ORVARK13@RU.IS",
                Email = "orvark13@ru.is",
                NormalizedEmail = "ORVARK13@RU.IS",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            userOrvar.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(userOrvar, "sesam");
            context.Users.Add(userOrvar);

            var userJohn = new ApplicationUser {
                UserName = "John Smith",
                NormalizedUserName = "JOHN SMITH",
                Email = "john@thesmiths.com",
                NormalizedEmail = "JOHN@THESMITHS.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            userJohn.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(userJohn, "jane4ever");
            context.Users.Add(userJohn);

            var userJane = new ApplicationUser {
                UserName = "Jane Smith",
                NormalizedUserName = "JANE SMITH",
                Email = "jane@thesmiths.com",
                NormalizedEmail = "JANE@THESMITHS.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            userJane.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(userJane, "h8john");
            context.Users.Add(userJane);


            var nodes = new Node[]
            {
                new Node {
                    Name = "README.txt",
                    ParentNodeID = -1,
                    ProjectID = 1,
                    CreatedByUserID = userOrvar.Id,
                    Type = NodeType.File,
                    CreatedDate = DateTime.Parse("2017-01-01")
                },
                new Node {
                    Name = "README.txt",
                    ParentNodeID = -1,
                    ProjectID = 2,
                    CreatedByUserID = userOrvar.Id,
                    Type = NodeType.File,
                    CreatedDate = DateTime.Parse("2017-05-05")
                }
            };

            foreach (Node n in nodes)
            {
                context.Nodes.Add(n);
            }

            var fileRevisions = new FileRevision[]
            {
                new FileRevision {
                    NodeID = 1,
                    Description = "Initial version",
                    Contents = System.Text.Encoding.UTF8.GetBytes("Hello World"),
                    CreatedByUserID = userOrvar.Id,
                    CreatedDate = DateTime.Parse("2017-04-04")
                }
            };

            foreach (FileRevision f in fileRevisions)
            {
                context.FileRevisions.Add(f);
            }

            var projects = new Project[]
            {
                new Project {
                    Name = "Default",
                    MainNodeID = 1,
                    CreatedByUserID = userOrvar.Id,
                    Template = true,
                    CreatedDate = DateTime.Parse("2017-01-01")
                },
                new Project { 
                    Name = "Testing",
                    MainNodeID = 2,
                    CreatedByUserID = userOrvar.Id,
                    Template = false,
                    CreatedDate = DateTime.Parse("2017-05-05")
                }
            };

            foreach (Project p in projects)
            {
                context.Projects.Add(p);
            }

            var sharings = new Sharing[]
            {
                new Sharing {
                    ProjectID = 1,
                    UserID = userJohn.Id
                },
                new Sharing {
                    ProjectID = 1,
                    UserID = userJane.Id
                },
                new Sharing {
                    ProjectID = 2,
                    UserID = userJane.Id
                },
            };

            foreach (Sharing s in sharings)
            {
                context.Sharings.Add(s);
            }

            context.SaveChanges();
        }
    }
}