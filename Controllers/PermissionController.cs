using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models;
using WebTools.Models.Entity;
using WebTools.Services.Interface;

namespace WebTools.Controllers
{
    public class PermissionController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IRolesServices _roleServices;

        public PermissionController(IUserServices userServices, IRolesServices roleServices)
        {
            _userServices = userServices;
            _roleServices = roleServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region User
        public IActionResult Users()
        {
            UsersViewModel model = new UsersViewModel();
            model.UsersList = _userServices.GetAllUsers();
            //model.Roles = new SelectList(_roleServices.GetAllRoles(), "RoleID", "RoleName");
            model.RolesList = _roleServices.GetAllRoles();
            return View(model);
        }

        [HttpGet]
        public IActionResult EditUsers(int? id)
        {
            UsersViewModel model = new UsersViewModel();
            if (id != null)
            {
                model.Users = _userServices.GetUsersByID(id);
                return PartialView("_EditUserPartial", model);
            }
            TempData["ErrorMsg"] = $"Lỗi! Không tìm thấy thông tin người dùng";
            return RedirectToAction("Users");
        }
        [HttpPost]
        public IActionResult EditUsers(Users users)
        {
            if (users.UserID > 0 && users.RoleID > 0)
            {
                var result = _userServices.EditUser(users);
                if (result == "OK")
                {
                    TempData["SuccessMsg"] = $"Người dùng: {users.UserName.ToUpper()} đã được cập nhật lại Role thành công!";
                }
                else
                {
                    TempData["ErrorMsg"] = "Lỗi! " + result;
                }
                return RedirectToAction("Users");
            }
            TempData["ErrorMsg"] = $"Lỗi! Không thể cập nhật lại Role cho người dùng {users.UserName.ToUpper()}";
            return RedirectToAction("Users");
        }

        [HttpGet]
        public IActionResult UserRoles(int? id)
        {
            var viewModel = new List<Roles>();
            var user = _userServices.GetUsersByID(id);
            foreach (var role in _roleServices.GetAllRoles())
            {
                var roles = new Roles
                {
                    RoleID = role.RoleID,
                    RoleName = role.RoleName,
                    Description = role.Description,
                };
                if (_userServices.IsUserInRole(user.UserID,role.RoleID))
                {
                    role.Status = true;
                }
                else
                {
                    role.Status = false;
                }
                viewModel.Add(role);
            }

            UsersViewModel model = new UsersViewModel();
            model.RolesList = viewModel;
            model.Users = _userServices.GetUsersByID(id);

            return PartialView("_UserRolesPartial", model);
        }
        #endregion

        #region Roles
        public IActionResult Roles()
        {
            RolesViewModel model = new RolesViewModel();
            model.RolesList = _roleServices.GetAllRoles();
            model.Roles = null;
            return View(model);
        }

        [HttpGet]
        public IActionResult EditRoles(int? id)
        {
            RolesViewModel model = new RolesViewModel();
            model.Roles = _roleServices.GetRolesByID(id);
            return PartialView("_EditRolePartial", model);
        }
        [HttpPost]
        public IActionResult EditRoles(Roles roles)
        {
            roles.ModifiedBy = User.Identity.Name;
            roles.DateModified = DateTime.Today;

            if (roles.RoleID > 0 && roles.RoleName != null)
            {
                var result = _roleServices.EditRoles(roles);
                if (result == "OK")
                {
                    TempData["SuccessMsg"] = $"Role: {roles.RoleName} đã được cập nhật lại thông tin thành công!";
                }
                else
                {
                    TempData["ErrorMsg"] = "Lỗi! " + result;
                }
                return RedirectToAction("Index");
            }
            TempData["ErrorMsg"] = $"Lỗi! Không tìm thấy thông tin Roles: {roles.RoleName}";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CreateRoles()
        {
            return PartialView("_AddRolePartial");
        }
        [HttpPost]
        public IActionResult CreateRoles(Roles roles)
        {
            roles.CreatedBy = User.Identity.Name;
            //roles.DateModified = DateTime.Today;

            if (roles.RoleName != null)
            {
                var result = _roleServices.AddRoles(roles);
                if (result == "OK")
                {
                    TempData["SuccessMsg"] = $"Thêm Role: {roles.RoleName} thành công!";
                }
                else
                {
                    TempData["ErrorMsg"] = "Lỗi! " + result;
                }
                return RedirectToAction("Index");
            }
            TempData["ErrorMsg"] = "Lỗi! Không thể thêm Roles";
            return RedirectToAction("Index");
        }

        public IActionResult DeleteRoles(int? id)
        {
            RolesViewModel model = new RolesViewModel();
            model.Roles = _roleServices.GetRolesByID(id);
            if (id != null)
            {
                var result = _roleServices.DeleteRoles(id);
                if (result == "OK")
                {
                    TempData["SuccessMsg"] = "Xóa Role: " + model.Roles.RoleName + " thành công!";
                }
                else
                {
                    TempData["ErrorMsg"] = "Lỗi! " + result;
                }
                return RedirectToAction("Index");
            }
            TempData["ErrorMsg"] = "Lỗi! Không tìm thấy ID Role";
            return RedirectToAction("Index");
        }
        #endregion

    }
}
