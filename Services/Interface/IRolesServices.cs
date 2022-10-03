using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models;

namespace WebTools.Services.Interface
{
    public interface IRolesServices
    {
        public List<Roles> GetAllRoles();

        public Roles GetRolesByID(int? id);

        public string AddRoles(Roles roles);

        public string EditRoles(Roles roles);

        public string DeleteRoles(int? id);
    }
}
