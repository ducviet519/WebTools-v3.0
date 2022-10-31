using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using WebTools.Models;
using WebTools.Services.Interface;

namespace WebTools.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        
        private readonly ILogger<HomeController> _logger;
        private readonly IGoogleDriveAPI _googleDriveAPI;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IGoogleDriveAPI googleDriveAPI, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _googleDriveAPI = googleDriveAPI;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UploadFileGG()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadFileGG(IFormFile fileUpload)
        {
            string fileGoogleID = "Upload không thành công";
            if (fileUpload != null && fileUpload.Length > 0)
            {
                string getDateS = DateTime.Now.ToString("ddMMyyyyHHmmss");
                string fileName = $"{getDateS}_{fileUpload.FileName}";
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Upload");
                string filePath = Path.Combine(uploadsFolder, fileName);

                fileGoogleID = _googleDriveAPI.UploadFile(filePath);
                ViewBag.Msg = fileGoogleID;
                return View();
            }
            else
            {
                ViewBag.Msg = fileGoogleID;
                return View();
            }
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
