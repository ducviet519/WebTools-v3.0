using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Security.Claims;
using System.Threading.Tasks;
using WebTools.Context;
using WebTools.Models;
using WebTools.Models.Entity;

namespace WebTools.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Denied()
        {
            return View();
        }

        private UserAccount CheckLoginDomain(string domain, string username, string password)
        {
            try
            {
                var activeDirectoryHelper = new ActiveDirectoryHelper(domain);

                if (activeDirectoryHelper.IsAuthenticated(domain, username, password))
                {
                    var userprincipal = string.Format(@"{0}\{1}", domain, username);

                    using PrincipalContext context = new(ContextType.Domain, domain, userprincipal, password);

                    var userInfo = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userprincipal);

                    return new UserAccount()
                    {
                        User_ID = 0, //string.IsNullOrEmpty(userInfo.Description) ? 0 : int.Parse(userInfo.Description),
                        DisplayName = userInfo.DisplayName,
                        UserName = $@"{username.Trim().ToLower()}@{domain}",
                        Password = password,
                        Source = domain,
                        Email = userInfo.EmailAddress,
                        Status = (userInfo.Enabled ?? false)
                    };
                }
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
            }
            return null;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewData["RerurnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid) 
            {
                TempData["Error"] = "Lỗi! Tài khoản hoặc mật khẩu không được bỏ trống";
                return View(login);
            }

            string domain = string.Empty;
            var isAuthorized = false;
            try
            {
                var userDomain = login.UserName;
                domain = "bvta.vn";
                //var user2 = CheckLoginDomain(domain, login.UserName, login.Password);
                //userDomain = $"{login.UserName}@{domain}";
                var userprincipal = $@"{domain}\{login.UserName}";

                using PrincipalContext context = new PrincipalContext(ContextType.Domain, domain, userprincipal, login.Password);

                UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, login.UserName);

                if (user == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin tài khoản.";
                }
                else
                {
                    isAuthorized = true;
                }
                string adPath = $"LDAP://{domain}";
                var ad_authenticate = new ActiveDirectoryHelper(adPath);
                //if (isAuthorized)
                if (ad_authenticate.IsAuthenticated(domain, login.UserName, login.Password))
                {
                    var UserName = login.UserName.Trim().ToLower();
                    var Password = login.Password.Trim();
                    var IP = HttpContext.Connection.RemoteIpAddress.ToString();
                    var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.NameIdentifier, UserName),
                    new Claim(ClaimTypes.GroupSid, string.IsNullOrEmpty(domain) ? "local" : domain),
                    new Claim(ClaimTypes.Email, user.EmailAddress ?? ""),
                    //new Claim(ClaimTypes.Expired, user2.Status ? "0" : "1"),
                };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                TempData["Error"] = "Lỗi! Tài khoản hoặc Mật khẩu không chính xác";
                return View();
            }
            catch(Exception ex)
            {
                TempData["Error"] = "Lỗi! Tài khoản hoặc Mật khẩu không chính xác";
                return View();
            }
        }
      
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
