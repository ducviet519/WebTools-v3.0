using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models.Entities
{
    public class RoleControllerActions: ModuleActions
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public int Controller_ID { get; set; }
        public string ControllerName { get; set; }
        

    }
}
