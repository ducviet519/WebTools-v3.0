using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models.Entities
{
    public class ModuleActions
    { 
        public int ActionID { get; set; }
        public string ActionName { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public int ControllerID { get; set; }
    }
}
