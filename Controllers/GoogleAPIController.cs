using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models;
using WebTools.Services;
using WebTools.Services.Interface;

namespace WebTools.Controllers
{
    [Authorize (Roles="Admin")]
    public class GoogleAPIController : Controller
    {
        private readonly IGoogleDriveAPI _googleDriveAPI;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public GoogleAPIController(IGoogleDriveAPI googleDriveAPI, IWebHostEnvironment webHostEnvironment)
        {
            _googleDriveAPI = googleDriveAPI;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            GoogleFilesViewModel model = new GoogleFilesViewModel();
            model.GoogleDriveList = await _googleDriveAPI.GetDriveFiles();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GoogleFiles()
        {
            GoogleFilesViewModel model = new GoogleFilesViewModel();
            model.GoogleDriveList = await _googleDriveAPI.GetDriveFiles();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SearchGoogleFiles()
        {
            GoogleFilesViewModel model = new GoogleFilesViewModel();
            model.GoogleDriveList = await _googleDriveAPI.GetDriveFiles();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SearchGoogleFiles(string searchString)
        {
            GoogleFilesViewModel model = new GoogleFilesViewModel();
            model.GoogleDriveList = (await _googleDriveAPI.SearchDriveFiles(searchString)).ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult UploadFileGG()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileGG(IFormFile fileUpload)
        {
            string fileGoogleID = "Upload không thành công";
            if (fileUpload != null && fileUpload.Length > 0)
            {
                string getDateS = DateTime.Now.ToString("ddMMyyyyHHmmss");
                string fileName = $"{getDateS}_{fileUpload.FileName}";
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Upload");
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    await fileUpload.CopyToAsync(fileStream);
                }
                fileGoogleID = _googleDriveAPI.UploadFile(filePath);                
                FileInfo file = new FileInfo(filePath);
                if (file.Exists && !String.IsNullOrEmpty(fileGoogleID) )//check file exsit or not  
                {
                    file.Delete();
                }
                ViewBag.Msg = $"Upload file thành công. File ID: {fileGoogleID}";
                return View();
            }
            else
            {
                ViewBag.Msg = fileGoogleID;
                return View();
            }
        }
    }
}
