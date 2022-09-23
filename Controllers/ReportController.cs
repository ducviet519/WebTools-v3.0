using GleamTech.DocumentUltimate.AspNet.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebTools.Context;
using WebTools.Models;
using WebTools.Services;
using WebTools.Services.Interface;

namespace WebTools.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
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



        public IActionResult Index
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
            List<ReportList> data = _reportListServices.GetReportList().ToList();

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
                data = data.Where(s => s.TenBM != null && s.TenBM.ToLower().Contains(SearchString.ToLower()) || s.MaBM != null && s.MaBM.ToUpper().Contains(SearchString.ToUpper())).ToList();
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
            return View(model);
        }

        //Sắp xếp
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

        //3. Tạo chức năng Lưu ở giao diện Thêm biểu mẫu
        [HttpGet]
        public IActionResult AddReport()
        {
            ReportListViewModel model = new ReportListViewModel();
            model.Depts = new SelectList(_depts.GetAll_Depts(), "STT", "KhoaP");
            return PartialView("_AddReportPartial", model);
        }

        [HttpPost]
         public IActionResult AddReport(ReportList reportList)
        {
            reportList.CreatedUser = "1";
            if (reportList.fileUpload != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Upload");
                string filePath = Path.Combine(uploadsFolder, reportList.fileUpload.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    reportList.fileUpload.CopyTo(fileStream);
                }
                reportList.FileLink = filePath;
                _reportListServices.InsertReportList(reportList);
                return RedirectToAction("Index");
            }
            else
            {
                _reportListServices.InsertReportList(reportList);
                return RedirectToAction("Index");
            }               
        }

        //4. Tạo chức năng hiển thị phiên bản
        public IActionResult Version(string id)
        {
            ReportVersionViewModel model = new ReportVersionViewModel();
            model.VersionList = _reportVersionServices.GetReportVersion(id).LastOrDefault();
            model.VersionLists = _reportVersionServices.GetReportVersion(id).ToList();

            return PartialView("_VesionPartial", model);
        }

        //5. Tạo chức năng Lưu phiên bản
        [HttpPost]
        public IActionResult AddVersion(ReportVersion reportVersion)
        {
            reportVersion.CreatedUser = "1";
            string IDBieuMau = reportVersion.IDBieuMau;
            string resault = string.Empty;
            if (reportVersion.IDBieuMau != null)
            {
                if (reportVersion.fileUpload != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Upload");
                    string filePath = Path.Combine(uploadsFolder, reportVersion.fileUpload.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        reportVersion.fileUpload.CopyTo(fileStream);
                    }
                    reportVersion.FileLink = filePath;
                    _reportVersionServices.InsertReportVersion(reportVersion);
                    return RedirectToAction("Index");
                }
                else
                {
                    _reportVersionServices.InsertReportVersion(reportVersion);
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        //6. Tạo cửa sổ Phần mềm
        public IActionResult Soft(string id)
        {
            ReportSoftViewModel model = new ReportSoftViewModel();
            model.ReportSoft = _reportSoftServices.GetReportSoft(id).FirstOrDefault();
            model.ReportSofts = _reportSoftServices.GetReportSoft(id).ToList();

            //URD
            var data = _reportURDServices.GetAll_URD().Select(x => new ReportURD()
            {
                Value = x.ID.ToString(),
                Text = x.Des
            }).ToList();
            model.URDLists = data;

            return PartialView("_SoftPartial", model);
        }

        //7. Tạo chức năng lưu dữ liệu khi ấn nút Lưu ở phần 6
        [HttpPost]
        public IActionResult AddSoft(ReportSoft reportSoft)
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
                reportSoft.User = "1";
                if (reportSoft.IDBieuMau != null)
                {
                    _reportSoftServices.InsertReportSoft(reportSoft);
                }
            }
            return RedirectToAction("Index");
        }

        //8. Tạo giao diện Chi tiết
        public IActionResult Detail(string id)
        {
            ReportDetailViewModel model = new ReportDetailViewModel();
            model.ReportDetail = _reportDetailServices.GetReportDetail(id).FirstOrDefault();
            model.ReportDetails = _reportDetailServices.GetReportDetail(id).ToList();
            model.Depts = new SelectList(_depts.GetAll_Depts(),"STT","KhoaP");
            return PartialView("_DetailPartial", model);
        }

        //9. Tạo chức năng lưu dữ liệu khi ấn nút Lưu ở phần 8
        public IActionResult AddDetail(ReportDetail reportDetail)
        {
            reportDetail.User = "1";
            int count = Int32.Parse(Request.Form["count"]);
            for (int i = 0; i < count; i++)
            {
                reportDetail.IDBieuMau = Request.Form["IDBieuMau"];
                reportDetail.IDPhienBan = Request.Form["IDPhienBan-" + i];
                reportDetail.KhoaPhong = Request.Form["KhoaPhong"];
                reportDetail.GhiChu = Request.Form["GhiChu"];
                reportDetail.TrangThai = Request.Form["TrangThai-" + i];
                reportDetail.User = "1";
                if (reportDetail.IDBieuMau != null)
                {
                    _reportDetailServices.InsertReportDetail(reportDetail);
                }

            }
            return RedirectToAction("Index");
        }

        //
        public IActionResult DocumentView(string link)
        {
            //DocumentViewModel model = new DocumentViewModel();
            //model.FileLink = $@"{link}";
            var documentLink = $@"{link}";
            var documentViewer = new DocumentViewer
            {
                Width = 1000,
                Height = 600,
                Resizable = false,
                Document = documentLink
            };

            return PartialView("_DocumentView", documentViewer);
        }
    }
}
