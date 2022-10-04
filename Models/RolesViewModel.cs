using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Models
{
    public class RolesViewModel
    {
        public Roles Roles { get; set; }

        public List<Roles> RolesList { get; set; }

        public ModuleControllers Controllers { get; set; }

        public List<ModuleControllers> ControllerLists { get; set; }
    }
}
