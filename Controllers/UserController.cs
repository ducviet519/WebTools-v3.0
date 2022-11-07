using AppBVTA.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebTools.Context;
using WebTools.Models;
using WebTools.Models.Entity;
using WebTools.Services.Interface;

namespace WebTools.Controllers
{  
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IRolesServices _roleServices;

        public UserController(IUserServices userServices, IRolesServices rolesServices)
        {
            _userServices = userServices;
            _roleServices = rolesServices;
        }

        public IActionResult Index()
        {     
            return View();
        }

        [HttpGet]
        public IActionResult Denied()
        {
            return View();
        }

        #region UserHelper
        private List<Claim> GenerateClaim(UserModel user)
        {
            //var UserLoginInfo = _userServices.FindByName(login.Username);
            //var RoleInUser = _userServices.GetRoleInUser(UserLoginInfo.UserID);
            //var PermissionsInUser = _userServices.GetAllUserPermissions(UserLoginInfo.UserName);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.DisplayName ?? "Người dùng vô danh"),
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.GroupSid, user.Source ?? "local"),
                new Claim(ClaimTypes.Email, user.EmailAddress ?? ""),
                new Claim(ClaimTypes.GivenName, user.DisplayName ?? ""),
                new Claim(ClaimTypes.Surname, user.Permission ?? ""),
                new Claim(ClaimTypes.Role, user.Role ?? ""),
            };

            //foreach (var role in RoleInUser)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role.RoleName));

            //}
            //foreach (var permission in PermissionsInUser)
            //{
            //    claims.Add(new Claim("Permission", $"{permission.Permission}"));
            //}

            return claims;
        }
        private UserModel GetUser(UserLogin login)
        {
            try
            {
                string domain = "bvta.vn";
                var userprincipal = $@"{domain}\{login.Username}";
                using PrincipalContext context = new PrincipalContext(ContextType.Domain, domain, userprincipal, login.Password);
                UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, login.Username);
                if (user == null)
                {
                    return null;
                }
                else
                {
                    UserModel userAccount = new UserModel()
                    {
                        DisplayName = user.DisplayName,
                        Username = login.Username.Trim().ToLower(),
                        Password = login.Password,
                        Source = domain,
                        EmailAddress = $@"{login.Username.Trim().ToLower()}@{domain}",
                        Status = (user.Enabled ?? false)
                    };

                    //Ghi thông tin người dùng vào CSDL
                    //_userServices.AddUser(userAccount);
                    return userAccount;
                }
            }
            catch (Exception ex)
            {
                var errorMsg = ex.Message;
                return null;
            }
        }

        #endregion

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewData["RerurnUrl"] = returnUrl;
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserLogin login)
        {
            if (String.IsNullOrEmpty(login.Username) || String.IsNullOrEmpty(login.Password))
            {
                TempData["Error"] = "Lỗi! Tài khoản hoặc mật khẩu không được bỏ trống";
                return View(login);
            }
            try
            {
                var UserLoginInfo = GetUser(login);
                if (UserLoginInfo != null)
                {
                    string adPath = $"LDAP://{UserLoginInfo.Source}";
                    var ad_authenticate = new ActiveDirectoryHelper(adPath);

                    if (ad_authenticate.IsAuthenticated(UserLoginInfo.Source, UserLoginInfo.Username, UserLoginInfo.Password))
                    {
                        List<Claim> claims = GenerateClaim(UserLoginInfo);
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
                            new AuthenticationProperties()
                            {
                                IsPersistent = UserLoginInfo.Status
                            });
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                    TempData["Error"] = "Lỗi! Tài khoản hoặc Mật khẩu không chính xác.";
                    return View();
                }
                else
                {
                    TempData["Error"] = "Lỗi! Tài khoản hoặc Mật khẩu không chính xác.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                var errorMsg = ex.Message;
                TempData["Error"] = $"Lỗi! {errorMsg}.";
                return View();
            }
        }
      
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
