using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models.Entities
{
    public class UserRoles:Roles
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
