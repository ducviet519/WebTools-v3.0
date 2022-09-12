using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using WebTools.Context;
using WebTools.Models;
using WebTools.Services;

namespace WebTools.Controllers
{
    public class ReportController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IReportListServices _reportListServices;
        private readonly IReportVersionServices _reportVersionServices;
        private readonly IReportSoftServices _reportSoftServices;
        private readonly IReportDetailServices _reportDetailServices;

        public ReportController(
            IConfiguration configuration,
            IReportListServices reportListServices,
            IReportVersionServices reportVersionServices,
            IReportSoftServices reportSoftServices,
            IReportDetailServices reportDetailServices
            )
        {
            _configuration = configuration;
            _reportListServices = reportListServices;
            _reportVersionServices = reportVersionServices;
            _reportSoftServices = reportSoftServices;
            _reportDetailServices = reportDetailServices;
        }



        public IActionResult Index
            (
            string sortField,
            string currentSortField,
            string currentSortOrder,
            string SearchString,
            string SearchTrangThaiSD,
            string SearchTrangThaiPM,
            DateTime SearchDate,
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
                //data = data.Where(s => s.TenBM.ToLower().Contains(SearchString.ToLower()) || s.MaBM.ToLower().Contains(SearchString.ToLower())).ToList();
                data = data.Where(s => s.TenBM.ToLower().Contains(SearchString.ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(SearchTrangThaiSD))
            {
                data = data.Where(s => s.TrangThai.ToLower().Contains(SearchTrangThaiSD.ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(SearchTrangThaiPM))
            {
                data = data.Where(s => s.TrangThaiPM.ToLower().Contains(SearchTrangThaiPM.ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(SearchDate.ToString()))
            {
                data = data.Where(s => s.NgayBanHanh.ToString("ddMMyyyy").Contains(SearchDate.ToString("ddMMyyyy"))).ToList();
            }

            //model.ReportLists = this.SortData(data, sortField, currentSortField, currentSortOrder);
            //return View(model);
            var a = this.SortData(data, sortField, currentSortField, currentSortOrder);
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

        [HttpGet]
        public IActionResult AddReport()
        {
            ReportListViewModel reportList = new ReportListViewModel();

            return PartialView("_AddReportPartial", reportList);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddReport(ReportList reportList)
        {
            ReportListViewModel model = new ReportListViewModel();
            reportList.CreatedUser = "1";
            string url = Request.Headers["Referer"].ToString();

            string resault = string.Empty;
            if(reportList.IDBieuMau != null)
            {
                resault = _reportListServices.UpdateReportList(reportList);
            }
            else
            {
                resault = _reportListServices.InsertReportList(reportList);
            }
            if(resault == "Inserted")
            {
                TempData["SuccessMsg"] = "Thêm biểu mẫu mới thành công";
                return RedirectToAction("Index");
                //return Redirect(url);
            }
            else
            {
                TempData["ErrorMsg"]= resault;
                return RedirectToAction("Index");
                //return Redirect(url);
            }
        }
    
        public IActionResult Version(string id)
        {
            ReportVersionViewModel model = new ReportVersionViewModel();
            model.VersionList = _reportVersionServices.GetReportVersion(id).FirstOrDefault();
            model.VersionLists = _reportVersionServices.GetReportVersion(id).ToList();

            return PartialView("_VesionPartial", model);
        }
        
        [HttpPost]
        public IActionResult AddVersion(ReportVersion reportVersion)
        {
            reportVersion.CreatedUser = "1";
            //string url = Request.Headers["Referer"].ToString();
            string IDBieuMau = reportVersion.IDBieuMau;
            string resault = string.Empty;
            if (reportVersion.IDBieuMau != null)
            {
                resault = _reportVersionServices.InsertReportVersion(reportVersion);
            }
            if (resault == "Inserted")
            {
                TempData["SuccessMsg"] = "Thêm Phiên bản mới thành công";
                return RedirectToAction("Index");
                //return Redirect(url);
            }
            else
            {
                TempData["ErrorMsg"] = resault;
                return RedirectToAction("Index");
                //return Redirect(url);
            }
        }

        public IActionResult Soft(string id)
        {
            ReportSoftViewModel model = new ReportSoftViewModel();
            model.ReportSoft = _reportSoftServices.GetReportSoft(id).FirstOrDefault();
            model.ReportSofts = _reportSoftServices.GetReportSoft(id).ToList();



            return PartialView("_SoftPartial", model);
        }
        [HttpPost]
        public IActionResult AddSoft(ReportSoft reportSoft)
        {
            //string url = Request.Headers["Referer"].ToString();
            reportSoft.User = "1";
            string resault = string.Empty;
            if (reportSoft.IDBieuMau != null)
            {
                resault = _reportSoftServices.InsertReportSoft(reportSoft);
            }
            if (resault == "Inserted")
            {
                TempData["SuccessMsg"] = "Thêm Phiên bản mới thành công";
                return RedirectToAction("Index");
                //return Redirect(url);
            }
            else
            {
                TempData["ErrorMsg"] = resault;
                return RedirectToAction("Index");
                //return Redirect(url);
            }
        }

        public IActionResult Detail(string id)
        {
            ReportDetailViewModel model = new ReportDetailViewModel();
            model.ReportDetail = _reportDetailServices.GetReportDetail(id).FirstOrDefault();
            model.ReportDetails = _reportDetailServices.GetReportDetail(id).ToList();

            return PartialView("_DetailPartial", model);
        }

        public IActionResult AddDetail(ReportDetail reportDetail)
        {
            //string url = Request.Headers["Referer"].ToString();
            reportDetail.User = "1";
            string resault = string.Empty;
            if (reportDetail.IDBieuMau != null)
            {
                resault = _reportDetailServices.InsertReportDetail(reportDetail);
            }
            if (resault == "Inserted")
            {
                TempData["SuccessMsg"] = "Thêm Phiên bản mới thành công";
                return RedirectToAction("Index");
                //return Redirect(url);
            }
            else
            {
                TempData["ErrorMsg"] = resault;
                return RedirectToAction("Index");
                //return Redirect(url);
            }
        }
    }
}
