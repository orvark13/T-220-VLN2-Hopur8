using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ode.Models;
using ode.Models.Entities;
using ode.Data;

namespace ode.Services
{
    public class FilesService
    {
        private readonly ApplicationDbContext _context;

        public FilesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Create(string fileName, int projectID, string userID, string contents)
        {
            var node = new Node()
            {
                Name = fileName,
                ParentNodeID = -1,
                ProjectID = projectID,
                CreatedByUserID = userID,
                Type = NodeType.File,
                CreatedDate = DateTime.Now
            };

            _context.Nodes.Add(node);
            _context.SaveChanges();

            var fileRevision = new FileRevision()
            {
                NodeID = node.ID,
                Description = "Revision of " + fileName + " saved " + DateTime.Now.ToString(),
                Contents = System.Text.Encoding.ASCII.GetBytes(contents),
                CreatedByUserID = userID,
                CreatedDate = DateTime.Now
            };

            _context.FileRevisions.Add(fileRevision);
            _context.SaveChanges();

            return node.ID;
        }

        public bool UpdateName(int projectID, string name)
        {
            if (name.Length > 0)
            {
                var project = _context.Projects
                    .Where(p => p.ID == projectID)
                    .SingleOrDefault();
                
                project.Name = name;

                _context.SaveChanges();

                return true;
            }
            return false;
        }
    }
}