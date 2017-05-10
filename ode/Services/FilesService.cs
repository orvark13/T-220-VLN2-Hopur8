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
        private readonly UsersService _usersService;
        
        public FilesService(ApplicationDbContext context, UsersService usersService)
        {
            _context = context;
            _usersService = usersService;
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
                Contents = System.Text.Encoding.UTF8.GetBytes(contents),
                CreatedByUserID = userID,
                CreatedDate = DateTime.Now
            };

            _context.FileRevisions.Add(fileRevision);
            _context.SaveChanges();

            return node.ID;
        }

        public int GetFilesProjectID(int nodeID)
        {
            return _context.Nodes.SingleOrDefault(n => n.ID == nodeID).ProjectID;
        }

        public FileViewModel GetFile(int nodeID)
        {
            var file = _context.Nodes
                .Where(n => n.ID == nodeID)
                .Select(x => new FileViewModel
                {
                    ID = x.ID,
                    Name = x.Name,
                    ParentNodeID = x.ParentNodeID,
                    ProjectID = x.ProjectID,
                    CreatedByUserID = x.CreatedByUserID,
                    CreatedDate = x.CreatedDate
                })
                .SingleOrDefault();
            
            file.CreatedByUser = _usersService.GetUserByID(file.CreatedByUserID);

            return file;
        }

        public FileRevisionViewModel GetFileRevision(int nodeID, int? fileRevisionID)
        {
            var revision = new FileRevisionViewModel();

            if (fileRevisionID == null)
            {
                revision = _context.FileRevisions
                    .Where(r => r.NodeID == nodeID)
                    .OrderByDescending(r => r.CreatedDate)
                    .Select(x => new FileRevisionViewModel
                    {
                        ID = x.ID,
                        NodeID = x.NodeID,
                        Description = x.Description,
                        Contents = System.Text.Encoding.UTF8.GetString(x.Contents),
                        CreatedByUserID = x.CreatedByUserID,
                        CreatedDate = x.CreatedDate
                    })
                    .SingleOrDefault();
            }
            else
            {
                revision = _context.FileRevisions
                    .Where(r => r.ID == fileRevisionID)
                    .Select(x => new FileRevisionViewModel
                    {
                        ID = x.ID,
                        NodeID = x.NodeID,
                        Description = x.Description,
                        Contents = System.Text.Encoding.UTF8.GetString(x.Contents),
                        CreatedByUserID = x.CreatedByUserID,
                        CreatedDate = x.CreatedDate
                    })
                    .SingleOrDefault();
            }

            return revision;
        }

        public void UpdateFileRevision(int nodeID, int fileRevisionID, string contents)
        {
            var revision = _context.FileRevisions
                .SingleOrDefault(r => r.ID == fileRevisionID);

            if (revision == null)
            {
                return;
            }

            revision.Contents = System.Text.Encoding.UTF8.GetBytes(contents);
            revision.Description = "Revision overwritten on " + DateTime.Now.ToString();
            _context.SaveChanges();
        }

        public void DeleteFileByID(int nodeID)
        {
            var node = _context.Nodes
                .SingleOrDefault(n => n.ID == nodeID);

            if (node == null)
            {
                return;
            }

            var revisions = _context.FileRevisions
                .Where(r => r.NodeID == nodeID);
            if (revisions != null)
            {
                _context.RemoveRange(revisions);
            }
            _context.Nodes.Remove(node);

            _context.SaveChanges();
        }

        public void DeleteFilesByProjectID(int projectID)
        {
            var nodes = _context.Nodes
                .Where(n => n.ProjectID == projectID)
                .ToList();
            
            foreach (var n in nodes)
            {
                var revisions = _context.FileRevisions
                    .Where(r => r.NodeID == n.ID);
                _context.RemoveRange(revisions);
            }

            _context.RemoveRange(nodes);

            _context.SaveChanges();
        }

        public bool UpdateName(int nodeID, string name)
        {
            if (name.Length > 0)
            {
                var node = _context.Nodes
                    .Where(p => p.ID == nodeID)
                    .SingleOrDefault();
                
                node.Name = name;

                _context.SaveChanges();

                return true;
            }
            return false;
        }
    }
}