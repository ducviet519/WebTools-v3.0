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
    }
}
