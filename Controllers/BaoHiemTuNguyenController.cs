using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;
using WebTools.Services.Interface;

namespace WebTools.Controllers
{
    [Authorize]
    public class BaoHiemTuNguyenController : Controller
    {
        private readonly IBaoHiemTuNguyenServices _baoHiemTuNguyenServices;
        public BaoHiemTuNguyenController(IBaoHiemTuNguyenServices baoHiemTuNguyenServices)
        {
            _baoHiemTuNguyenServices = baoHiemTuNguyenServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult KhachHangThanhToanBHTN()
        {
            return View();
        }
        public IActionResult BangKeChiPhi()
        {
            return View();
        }
        public async Task<JsonResult> SearchKhanhHangThanhToanBHTN(string mabn, string loai, string ngayBD, string ngayKT)
        {
            var data = (await _baoHiemTuNguyenServices.SearchKhanhHangThanhToanBHTN(mabn, loai, ngayBD, ngayKT)).ToList();
            return Json(new { data });
        }
    }
}
