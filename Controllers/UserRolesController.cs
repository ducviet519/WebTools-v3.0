using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models;
using WebTools.Models.Entity;
using WebTools.Services.Interface;

namespace WebTools.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    public class UserRolesController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IRolesServices _rolesServices;

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRolesController(IUserServices userServices, IRolesServices rolesServices
            , UserManager<IdentityUser> userManager
            , SignInManager<IdentityUser> signInManager
            , RoleManager<IdentityRole> roleManager
            )
        {
            _userServices = userServices;
            _rolesServices = rolesServices;

            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index(int userId)
        {
            var viewModel = new List<Roles>();
            var user = _userServices.GetUsersByID(userId);
            foreach (var role in _rolesServices.GetAllRoles())
            {
                var rolesData = new Roles
                {   RoleID = role.RoleID,
                    RoleName = role.RoleName,
                    Description = role.Description
                };
                if (true)
                {
                    rolesData.Status = true;
                }
                else
                {
                    rolesData.Status = false;
                }
                viewModel.Add(rolesData);
            }
            var model = new UsersViewModel()
            {
                UserID = userId,
                RolesList = viewModel
            };
            return View(model);
        }
        
    }
}
