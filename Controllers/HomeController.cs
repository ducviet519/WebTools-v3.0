using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using WebTools.Models;
using WebTools.Services.Interface;

namespace WebTools.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        
        private readonly ILogger<HomeController> _logger;
        private readonly IGoogleDriveAPI _googleDriveAPI;

        public HomeController(ILogger<HomeController> logger, IGoogleDriveAPI googleDriveAPI)
        {
            _logger = logger;
            _googleDriveAPI = googleDriveAPI;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UploadFileGG()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFileGG(IFormFile file)
        {           
            string fileGoogleID = _googleDriveAPI.UploadFile(file);
            ViewBag.Success = "File Uploaded on Google Drive";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
