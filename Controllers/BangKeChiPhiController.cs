using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models;
using WebTools.Models.Entities;
using WebTools.Services;
using WebTools.Services.Interface;

namespace WebTools.Controllers
{
    public class BangKeChiPhiController : Controller
    {
        public List<BangKeChiPhi> bangKeChiPhis = new List<BangKeChiPhi>();
        private readonly IBangKeChiPhiSevices _bangKeChiPhiSevices;
        private readonly IDanhMucBHTNServices _danhMucBHTNServices;
        public BangKeChiPhiController(IBangKeChiPhiSevices bangKeChiPhiSevices, IDanhMucBHTNServices danhMucBHTNServices)
        {
            _bangKeChiPhiSevices = bangKeChiPhiSevices;
            _danhMucBHTNServices = danhMucBHTNServices;
        }

        [HttpGet]
        public async Task<IActionResult> PhieuThanhToan(string id, string loai)
        {
            BangKeChiPhiVM model = new BangKeChiPhiVM();
            model.DonViBaoLanh = new SelectList(await _danhMucBHTNServices.GetDanhMucBHTN("1"), "ID", "Name");
            model.KhoaPhong = new SelectList(await _danhMucBHTNServices.GetDanhMucBHTN("2"), "ID", "Name");
            model.ListChiPhi = await (_bangKeChiPhiSevices.GetBangKeChiPhi(id, loai));
            model.BangKeChiPhi = (await (_bangKeChiPhiSevices.GetBangKeChiPhi(id, loai))).LastOrDefault();
            model.id = id;
            model.loai = loai;
            return PartialView("_PhieuThanhToan", model);
        }

        [HttpGet]
        public async Task<JsonResult> GetPhieuThanhToan(string id, string loai)
        {
            var bangKeChiPhis = (await (_bangKeChiPhiSevices.GetBangKeChiPhi(id, loai))).ToList();          
            return Json(new { data = bangKeChiPhis });
        }
        [HttpPost]
        public JsonResult AddPhieuThanhToan()
        {
            Int32.TryParse(Request.Form["count"], out int count);
            string message = "";
            string title = "";
            string result = "";
            try
            {
                for (var i = 0; i <= count; i++)
                {
                    if (!String.IsNullOrEmpty(Request.Form["SoLuongTT-" + i]) && !String.IsNullOrEmpty(Request.Form["BNTTThanhToan-" + i]))
                    {
                        var data = new BangKeChiPhi()
                        {
                            hoten = Request.Form["HoTen"],
                            mabn = Request.Form["MaKH"],
                            soluongtt = Request.Form["SoLuongTT-" + i],
                            bntra = Request.Form["BNTTThanhToan-" + i],
                        };
                        var test = data; //Sử dụng để truyền dữ liệu về services
                    }
                }
                if (result == "OK")
                {
                    message = $"Thành công! Đã thêm thành công";
                    title = "Thành công!";
                    result = "success";
                }
                else
                {
                    message = $"Lỗi! {result}";
                    title = "Lỗi!";
                    result = "error";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                title = "Lỗi!";
                result = "error";
            }
            return Json(new { Result = result, Title = title, Message = message });
        }
    }
}
