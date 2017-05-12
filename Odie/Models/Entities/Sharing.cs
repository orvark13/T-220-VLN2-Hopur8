using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Odie.Models.Entities
{
    /// <summary>
    /// Sharing, which projects have been shared with wich users.
    /// Needed so EF would create the corresponding db table.
    /// </summary>
    public class Sharing
    {
        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProjectID { get; set; }

        //[Key]
        [MaxLength(450)]
        public string UserID { get; set; }

    }

    /// <summary>
    /// Helper entity used in a Linq inner join.
    /// </summary>
    public class ProjectSharingJoin
    {
        public Project P { get; set; }
        public Sharing S { get; set; }
    }
}