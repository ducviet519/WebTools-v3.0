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

        public ReportController(IConfiguration configuration, IReportListServices reportListServices)
        {
            _configuration = configuration;
            _reportListServices = reportListServices;
        }



        public IActionResult Index(string sortField, string currentSortField, string currentSortOrder, string SearchString, string SearchTrangThaiSD, string SearchTrangThaiPM, DateTime SearchDate)
        {

            ReportListViewModel model = new ReportListViewModel();
            List<ReportList> data = _reportListServices.GetReportList().ToList();

            //Tìm kếm
            if (!String.IsNullOrEmpty(SearchString))
            {
                data = data.Where(s => s.TenBM.ToLower().Contains(SearchString.ToLower()) || s.MaBM.ToLower().Contains(SearchString.ToLower())).ToList();
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

            model.ReportLists = this.SortData(data, sortField, currentSortField, currentSortOrder);
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
                return Redirect(url);
            }
            else
            {
                TempData["ErrorMsg"]= resault;
                return Redirect(url);
            }
        }
    }
}
