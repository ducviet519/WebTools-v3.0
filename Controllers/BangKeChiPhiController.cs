﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly IDepts _depts;
        public BangKeChiPhiController(IBangKeChiPhiSevices bangKeChiPhiSevices, IDepts depts)
        {
            _bangKeChiPhiSevices = bangKeChiPhiSevices;
            _depts = depts;
        }

        public async Task<IActionResult> Index(string id= "221026140034723054")
        {
            BangKeChiPhiVM model = new BangKeChiPhiVM();
            model.Depts = new SelectList(await _depts.GetAll_DeptsAsync(), "KhoaP", "KhoaP");
            ViewBag.id = id;
            model.bangKeChiPhis = await (_bangKeChiPhiSevices.GetBangKeChiPhi(id, "1"));
            model.bangKeChiPhi = (await (_bangKeChiPhiSevices.GetBangKeChiPhi(id, "1"))).LastOrDefault();
            return View(model);
        }
        public async Task<IActionResult> BangKe(string id)
        {
            BangKeChiPhiVM model = new BangKeChiPhiVM();
            model.Depts = new SelectList(await _depts.GetAll_DeptsAsync(), "KhoaP", "KhoaP");
            ViewBag.id = id;
            model.bangKeChiPhis = await (_bangKeChiPhiSevices.GetBangKeChiPhi(id, "1"));
            model.bangKeChiPhi = (await (_bangKeChiPhiSevices.GetBangKeChiPhi(id, "1"))).LastOrDefault();
            return PartialView("BangKe",model);
        }
        //public async Task<IActionResult> PhieuThanhToan(string id)
        //{
        //    ViewBag.id = id;
        //    bangKeChiPhis = await (_bangKeChiPhiSevices.GetBangKeChiPhi(id, "2"));
        //    if(bangKeChiPhis.Count > 0)
        //    {
        //        return View(bangKeChiPhis);
        //    }
        //    else
        //    {
        //        bangKeChiPhis = new List<BangKeChiPhi>();
        //        return View(bangKeChiPhis);
        //    }
            
        //}
        [HttpGet]
        public async Task<JsonResult> GetBangKeChiPhi1(string id)
        {
            var bangKeChiPhis = (await (_bangKeChiPhiSevices.GetBangKeChiPhi(id, "1"))).ToList();
            return Json( new { data = bangKeChiPhis}  );
        }

        [HttpGet]
        public async Task<JsonResult> GetPhieuThanhToan(string id)
        {
            var bangKeChiPhis = (await (_bangKeChiPhiSevices.GetBangKeChiPhi(id, "2"))).ToList();
            
            return Json(new { data = bangKeChiPhis });
        }

        public async Task<IActionResult> PhieuThanhToan(string id)
        {
            id = "220816140019638226";
            BangKeChiPhiVM model = new BangKeChiPhiVM();
            model.Depts = new SelectList(await _depts.GetAll_DeptsAsync(), "KhoaP", "KhoaP");
            ViewBag.id = id;
            model.bangKeChiPhi = (await _bangKeChiPhiSevices.GetBangKeChiPhi(id, "2")).LastOrDefault();
            return PartialView("PhieuThanhToan", model);
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
