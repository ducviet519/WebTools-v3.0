using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebTools.Models;
using WebTools.Models.Entity;

namespace WebTools.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles ="Admin")]
        public IActionResult TestAuthorize()
        {
            return View();
        }

        [HttpGet("denied")]
        public IActionResult Denied()
        {
            return View();
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Validate(LoginViewModel model)
        {
            ViewData["ReturnUrl"] = model.ReturnUrl;
            string domain = string.Empty;
            if(model.UserName == "admin" && model.Password == "admin")
            {
                model.UserName = model.UserName.Trim().ToLower();
                model.Password = model.Password.Trim();
                var userDomain = model.UserName;

                var claims = new List<Claim>();
                claims.Add(new Claim("UserName", model.UserName));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, model.UserName));
                claims.Add(new Claim(ClaimTypes.GroupSid, string.IsNullOrEmpty(domain) ? "local" : domain));
                claims.Add(new Claim(ClaimTypes.Role,"Admin"));
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                return Redirect(model.ReturnUrl);
            }
            TempData["Error"] = "Lỗi! Tài khoản hoặc Mật khẩu không chính xác";
            return View("login");
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
