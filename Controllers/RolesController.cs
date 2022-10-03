using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models;
using WebTools.Services.Interface;

namespace WebTools.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRolesServices _rolesServices;

        public RolesController(IRolesServices rolesServices)
        {
            _rolesServices = rolesServices;
        }

        public IActionResult Index()
        {
            RolesViewModel model = new RolesViewModel();
            model.RolesList = _rolesServices.GetAllRoles();
            model.Roles = null;
            return View(model);
        }

        [HttpGet]
        public IActionResult EditRoles(int? id)
        {
            RolesViewModel model = new RolesViewModel();
            model.Roles = _rolesServices.GetRolesByID(id);
            return PartialView("_EditRolePartial", model);
        }
        [HttpPost]
        public IActionResult EditRoles(Roles roles)
        {
            roles.ModifiedBy = User.Identity.Name;
            roles.DateModified = DateTime.Today;

            if (roles.RoleID > 0 && roles.RoleName != null)
            {
                var result = _rolesServices.EditRoles(roles);
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
                var result = _rolesServices.AddRoles(roles);
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
            model.Roles = _rolesServices.GetRolesByID(id);
            if (id != null)
            {
                var result = _rolesServices.DeleteRoles(id);
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
    }
}
