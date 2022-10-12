using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Controllers
{
    public class BenhCXKController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PhieuKhamAsync()
        {
            return View();
        }

        public async Task<IActionResult> PhieuKhamCXKAsync()
        {
            return View();
        }

        public async Task<IActionResult> PhieuASDASAsync()
        {
            return View();
        }

        public async Task<IActionResult> PhieuDAPSAAsync()
        {
            return View();
        }

        public async Task<IActionResult> PhieuPASIAsync()
        {
            return View();
        }
    }
}
