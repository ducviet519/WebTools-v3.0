﻿using GleamTech.DocumentUltimate.AspNet.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebTools.Context;
using WebTools.Models;
using WebTools.Services;
using WebTools.Services.Interface;

namespace WebTools.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        #region Connnection Database

        private readonly IConfiguration _configuration;
        private readonly IReportListServices _reportListServices;
        private readonly IReportVersionServices _reportVersionServices;
        private readonly IReportSoftServices _reportSoftServices;
        private readonly IReportDetailServices _reportDetailServices;
        private readonly IReportURDServices _reportURDServices;
        private readonly IDepts _depts ;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportController(
            IConfiguration configuration,
            IReportListServices reportListServices,
            IReportVersionServices reportVersionServices,
            IReportSoftServices reportSoftServices,
            IReportDetailServices reportDetailServices,
            IReportURDServices reportURDServices,
            IDepts depts,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _configuration = configuration;
            _reportListServices = reportListServices;
            _reportVersionServices = reportVersionServices;
            _reportSoftServices = reportSoftServices;
            _reportDetailServices = reportDetailServices;
            _reportURDServices = reportURDServices;           
            _depts = depts;
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion

        #region Index Page

        public async Task<IActionResult> Index
            (
            string sortField,
            string currentSortField,
            string currentSortOrder,
            string SearchString,
            string SearchTrangThaiSD,
            string SearchTrangThaiPM,
            string SearchDate,
            //DateTime? SearchDate,
            string currentFilter,
            int? pageNo
            )
        {
            ReportListViewModel model = new ReportListViewModel();
            List<ReportList> data = await _reportListServices.GetReportListAsync();



            if (SearchString != null)
            {
                pageNo = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            ViewData["CurrentSort"] = sortField;
            ViewBag.CurrentFilter = SearchString;

            //Tìm kếm
            if (!String.IsNullOrEmpty(SearchString))
            {
                data = data.Where(s => s.TenBM != null && convertToUnSign(s.TenBM.ToLower()).Contains(convertToUnSign(SearchString.ToLower())) || s.MaBM != null && s.MaBM.ToUpper().Contains(SearchString.ToUpper())).ToList();
            }
            if (!String.IsNullOrEmpty(SearchTrangThaiSD))
            {
                data = data.Where(s => s.TrangThai.ToLower().Contains(SearchTrangThaiSD.ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(SearchTrangThaiPM))
            {
                data = data.Where(s => s.TrangThaiPM.ToLower().Contains(SearchTrangThaiPM.ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(SearchDate))
            {
                data = data.Where(s => s.NgayBanHanh != null &&  s.NgayBanHanh.Contains(SearchDate.ToString())).ToList();
            }

            var a = this.SortData(data, sortField, currentSortField, currentSortOrder);
            model.ReportLists = data;
            int pageSize = 10;
            model.PagingLists = PagingList<ReportList>.CreateAsync(a.AsQueryable<ReportList>(), pageNo ?? 1, pageSize);
            TempData["SearchString"] = SearchString;
            TempData["SearchDate"] = SearchDate;
            return View(model);
        }
        #endregion

        #region Khử dấu cho string        
        public static string convertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        #endregion
        #region Sắp xếp
        private List<ReportList> SortData(List<ReportList> model, string sortField, string currentSortField, string currentSortOrder)
        {
            if (string.IsNullOrEmpty(sortField))
            {
                ViewBag.SortField = "STT";
                ViewBag.SortOrder = "Asc";
            }
            else
            {
                if (currentSortField.ToLower() == sortField.ToLower())
                {
                    ViewBag.SortOrder = currentSortOrder == "Asc" ? "Desc" : "Asc";
                }
                else
                {
                    ViewBag.SortOrder = "Asc";
                }
                ViewBag.SortField = sortField;
            }

            var propertyInfo = typeof(ReportList).GetProperty(ViewBag.SortField);
            if (ViewBag.SortOrder == "Asc")
            {
                model = model.OrderBy(s => propertyInfo.GetValue(s, null)).ToList();
            }
            else
            {
                model = model.OrderByDescending(s => propertyInfo.GetValue(s, null)).ToList();
            }
            return model;
        }
        #endregion

        //3. Tạo chức năng Lưu ở giao diện Thêm biểu mẫu
        [HttpGet]
        public async Task<IActionResult> AddReport()
        {
            ReportListViewModel model = new ReportListViewModel();
            model.Depts = new SelectList(await _depts.GetAll_DeptsAsync(), "STT", "KhoaP");
            return PartialView("_AddReportPartial", model);
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> AddReport(ReportList reportList)
            {
            var data = await _reportListServices.GetReportListAsync();
            data = data.Where(r => r.MaBM != null && r.MaBM.ToUpper() == reportList.MaBM.ToUpper()).ToList();
            if (data.Count > 0) 
            {
                TempData["ErrorMsg"] = $"Lỗi! Mã biểu mẫu: {reportList.MaBM} đã tồn tại. Xin vui lòng kiểm tra lại";
                return RedirectToAction("Index");
            }
            else
            {
                string getDateS = DateTime.Now.ToString("ddMMyyyyHHmmss");
                reportList.KhoaPhong = Request.Form["KhoaPhong"];
                reportList.CreatedUser = User.Identity.Name;
                if (reportList.fileUpload != null && reportList.fileUpload.Length > 0)
                {
                    string fileName = $"{getDateS}_{reportList.fileUpload.FileName}";
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Upload");
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                    {
                        await reportList.fileUpload.CopyToAsync(fileStream);
                    }
                    reportList.FileLink = filePath;
                    var result = await _reportListServices.InsertReportListAsync(reportList);
                    if (result == "OK")
                    {
                        TempData["SuccessMsg"] = "Thêm biểu mẫu: " + reportList.TenBM + " thành công!";
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Lỗi! " + result;
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    var result = await _reportListServices.InsertReportListAsync(reportList);
                    if (result == "OK")
                    {
                        TempData["SuccessMsg"] = "Thêm biểu mẫu: " + reportList.TenBM + " thành công!";
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Lỗi! " + result;
                    }
                    return RedirectToAction("Index");
                }
            }          
        }
       

        [HttpGet]
        public async Task<IActionResult> EditReport(string id)
        {
            ReportListViewModel model = new ReportListViewModel();
            model.ReportList = await _reportListServices.GetReportByIDAsync(id);
            model.Depts = new SelectList(await _depts.GetAll_DeptsAsync(), "STT", "KhoaP");
            return PartialView("_EditReportPartial", model);
        }
        [HttpPost]
        public async Task<IActionResult> EditReport(ReportList reportList)
        {
            string getDateS = DateTime.Now.ToString("ddMMyyyyHHmmss");
            reportList.KhoaPhong = Request.Form["KhoaPhong"];
            reportList.CreatedUser = User.Identity.Name;
            if (reportList.fileUpload != null && reportList.fileUpload.Length > 0)
            {
                string fileName = $"{getDateS}_{reportList.fileUpload.FileName}";
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Upload");
                string filePath = Path.Combine(uploadsFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    await reportList.fileUpload.CopyToAsync(fileStream);
                }
                reportList.FileLink = filePath;
                var result = await _reportListServices.UpdateReportListAsync(reportList);
                if (result == "OK")
                {
                    TempData["SuccessMsg"] = "Cập nhật thông tin Biểu mẫu: " + reportList.TenBM + " thành công!";
                }
                else
                {
                    TempData["ErrorMsg"] = "Lỗi! " + result;
                }
                return RedirectToAction("Index");
            }
            else
            {
                var result = await _reportListServices.UpdateReportListAsync(reportList);
                if (result == "OK")
                {
                    TempData["SuccessMsg"] = "Cập nhật thông tin Biểu mẫu: " + reportList.TenBM + " thành công!";
                }
                else
                {
                    TempData["ErrorMsg"] = "Lỗi! " + result;
                }
                return RedirectToAction("Index");
            }
        }
        //4. Tạo chức năng hiển thị phiên bản
        public async Task<IActionResult> Version(string id)
        {
            ReportVersionViewModel model = new ReportVersionViewModel();
            model.VersionList = (await _reportVersionServices.GetReportVersionAsync(id)).LastOrDefault();
            model.VersionLists = (await _reportVersionServices.GetReportVersionAsync(id)).ToList();

            return PartialView("_VesionPartial", model);
        }

        //5. Tạo chức năng Lưu phiên bản
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> AddVersion(ReportVersion reportVersion)
        {
            var data = (await _reportVersionServices.GetReportVersionAsync(reportVersion.IDBieuMau)).Where(v => v.PhienBan.Contains(reportVersion.PhienBan));
            if (data.Any()) { TempData["ErrorMsg"] = $"Lỗi! Biểu mẫu đã tồn tại phiên bản: {reportVersion.PhienBan} xin vui lòng kiểm tra lại"; return RedirectToAction("Index"); }
            else
            {
                reportVersion.CreatedUser = User.Identity.Name;
                string getDateS = DateTime.Now.ToString("ddMMyyyy");
                string IDBieuMau = reportVersion.IDBieuMau;
                string resault = string.Empty;
                if (reportVersion.fileUpload != null && reportVersion.fileUpload.Length > 0)
                {
                    string fileName = $"{getDateS}_{reportVersion.MaBM}_{reportVersion.PhienBan}_{reportVersion.fileUpload.FileName}";
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Upload");
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        reportVersion.fileUpload.CopyTo(fileStream);
                    }
                    reportVersion.FileLink = filePath;
                    var result = await _reportVersionServices.InsertReportVersionAsync(reportVersion);
                    if (result == "OK")
                    {
                        TempData["SuccessMsg"] = "Thêm phiên bản thành công!";
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Lỗi! " + result;
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    var result = await _reportVersionServices.InsertReportVersionAsync(reportVersion);
                    if (result == "OK")
                    {
                        TempData["SuccessMsg"] = "Thêm phiên bản " + reportVersion.PhienBan + " cho biểu mẫu " + reportVersion.TenBM + " thành công!";
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Lỗi!" + result;
                    }
                    return RedirectToAction("Index");
                }
            }
        }

        public async Task<IActionResult> DeleteVersion(string IDPhienBan, string IDBieuMau)
        {
            string url = Request.Headers["Referer"].ToString();
            var result =  await _reportVersionServices.DeleteReportVersionAsync(IDPhienBan);
            if (result == "DEL")
            {
                TempData["SuccessMsg"] = "Xóa phiên bản thành công!";
                //return Json(new {message = "Del" });
            }
            else
            {
                TempData["ErrorMsg"] = "Lỗi!" + result;
                //return Json(new { message = "Lỗi! Phiên bản chưa được xóa" });
            }
            return RedirectToAction("Index");
        }

        //6. Tạo cửa sổ Phần mềm
        public async Task<IActionResult> Soft(string id)
        {
            ReportSoftViewModel model = new ReportSoftViewModel();
            model.ReportSoft = (await _reportSoftServices.GetReportSoftAsync(id)).FirstOrDefault();
            model.ReportSofts = (await _reportSoftServices.GetReportSoftAsync(id)).ToList();

            //URD SelectList
            model.URDs = new SelectList(await _reportURDServices.GetAll_URDAsync(), "ID", "Des");

            //Softs SelectList
            var PhanMems = new List<PhanMems>
            {
                new PhanMems{ID =  1, Name = "HIS" },
                new PhanMems{ID =  2, Name = "IVF" },
                new PhanMems{ID =  3, Name = "HRM" },
                new PhanMems{ID =  4, Name = "PM Kế toán" },
            };
            model.PhanMems = new SelectList(PhanMems, "Name", "Name");

            return PartialView("_SoftPartial", model);
        }

        //7. Tạo chức năng lưu dữ liệu khi ấn nút Lưu ở phần 6
        [HttpPost]
        public async Task<IActionResult> AddSoft(ReportSoft reportSoft)
        {
            string url = Request.Headers["Referer"].ToString();
            int count = Int32.Parse(Request.Form["count"]);
            for (int i = 0; i < count; i++)
            {
                reportSoft.IDBieuMau = Request.Form["IDBieuMau"];
                reportSoft.IDPhienBan = Request.Form["IDPhienBan-" + i];
                reportSoft.PhanMem = Request.Form["PhanMem"];
                reportSoft.URD = Request.Form["URD"];
                reportSoft.ViTriIn = Request.Form["ViTriIn"];
                reportSoft.CachIn = Request.Form["CachIn"];
                reportSoft.TrangThaiPM = Request.Form["TrangThaiPM-"+i];
                reportSoft.User = User.Identity.Name;
                if (reportSoft.IDBieuMau != null)
                {
                    var result = await _reportSoftServices.InsertReportSoftAsync(reportSoft);
                    if (result == "OK")
                    {
                        TempData["SuccessMsg"] = "Cập nhật thông tin thành công!";
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Lỗi!" + result;
                    }
                }
            }
            return RedirectToAction("Index");
        }

        //8. Tạo giao diện Chi tiết
        public async Task<IActionResult> Detail(string id)
        {
            ReportDetailViewModel model = new ReportDetailViewModel();
            model.ReportDetail = (await _reportDetailServices.GetReportDetailAsync(id)).FirstOrDefault();
            model.ReportDetails = (await _reportDetailServices.GetReportDetailAsync(id)).ToList();
            model.Depts = new SelectList(await _depts.GetAll_DeptsAsync(),"STT","KhoaP");
            return PartialView("_DetailPartial", model);
        }

        //9. Tạo chức năng lưu dữ liệu khi ấn nút Lưu ở phần 8
        public async Task<IActionResult> AddDetail(ReportDetail reportDetail)
        {
            reportDetail.User = User.Identity.Name;
            int count = Int32.Parse(Request.Form["count"]);
            for (int i = 0; i < count; i++)
            {
                reportDetail.IDBieuMau = Request.Form["IDBieuMau"];
                reportDetail.IDPhienBan = Request.Form["IDPhienBan-" + i];
                reportDetail.KhoaPhong = Request.Form["KhoaPhong"];
                reportDetail.GhiChu = Request.Form["GhiChu-" + i];
                reportDetail.TrangThai = Request.Form["TrangThai-" + i];
                reportDetail.User = "1";
                if (reportDetail.IDBieuMau != null)
                {
                    var result = await _reportDetailServices.InsertReportDetailAsync(reportDetail);
                    if (result == "OK")
                    {
                        TempData["SuccessMsg"] = "Cập nhật thông tin thành công!";
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Lỗi!" + result;
                    }
                }

            }
            return RedirectToAction("Index");
        }

        //Document View
        public IActionResult DocumentView(string link)
        {
            //DocumentViewModel model = new DocumentViewModel();
            //model.FileLink = $@"{link}";
            var documentLink = $@"{link}";
            var documentViewer = new DocumentViewer
            {
                Width = 1200,
                Height = 600,
                Resizable = false,
                Document = documentLink
            };

            return PartialView("_DocumentView", documentViewer);
        }
        //Document View
        public IActionResult PopUpDocumentView(string link)
        {
            //DocumentViewModel model = new DocumentViewModel();
            //model.FileLink = $@"{link}";
            var documentLink = $@"{link}";
            var documentViewer = new DocumentViewer
            {
                Width = 1100,
                Height = 600,
                Resizable = false,
                Document = documentLink
            };

            return PartialView("_DocumentViewPartial", documentViewer);
        }
    }
}
