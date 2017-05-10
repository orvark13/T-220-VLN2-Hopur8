using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ode.Models.Entities
{
    /// <summary>
    /// TODO
    /// </summary>
    public class Sharing
    {
        /// <summary>
        /// 
        /// </summary>
        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProjectID { get; set; }

        //[Key]
        [MaxLength(450)]
        public string UserID { get; set; }

    }

    public class ProjectSharingJoin
    {
        public Project P { get; set; }
        public Sharing S { get; set; }
    }
}