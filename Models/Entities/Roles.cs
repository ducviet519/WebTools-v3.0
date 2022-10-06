using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models
{
    public class Roles
    {

        public int RoleID { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }

        public bool Status { get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

    }
    
}
