using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using WebTools.Context;
using WebTools.Models;
using WebTools.Services;

namespace WebTools.Controllers
{
    public class ReportListController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IReportListServices _reportListServices;

        public ReportListController(IConfiguration configuration, IReportListServices reportListServices)
        {
            _configuration = configuration;
            _reportListServices = reportListServices;
        }



        public IActionResult Index()
        {
            ReportListViewModel model = new ReportListViewModel();
            model.ReportLists = _reportListServices.GetReportList().ToList();
            return View(model);
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
