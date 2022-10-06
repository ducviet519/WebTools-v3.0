using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Permission
{
    internal class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; private set; }
        public int RoleID { get; set; }
        public int ControllerID { get; set; }
        public int ActionID { get; set; }
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
