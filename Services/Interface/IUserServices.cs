using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;
using WebTools.Models.Entity;

namespace WebTools.Services.Interface
{
    public interface IUserServices
    {
        public List<Users> GetAllUsers();

        public Users GetUsersByID(int? id);

        public string AddUser(Users users);

        public string Delete(int id);

        public bool IsUserInRole(int UserID, int RoleID);

        public string EditUserRoles(UserRoles userRoles);

    }
}
