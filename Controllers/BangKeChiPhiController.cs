using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models;
using WebTools.Models.Entities;
using WebTools.Services;
using WebTools.Services.Interface;

namespace WebTools.Controllers
{
    [Authorize]
    public class BangKeChiPhiController : Controller
    {
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
            model.ListChiPhi = await _bangKeChiPhiSevices.GetBangKeChiPhi(id, loai, "1");
            model.BangKeChiPhi = (await _bangKeChiPhiSevices.GetBangKeChiPhi(id, loai, "1")).LastOrDefault();
            model.id = id;
            model.loai = loai;
            return PartialView("_PhieuThanhToan", model);
        }

        [HttpGet]
        public async Task<JsonResult> GetPhieuThanhToan(string id, string loai)
        {
            var bangKeChiPhis = (await (_bangKeChiPhiSevices.GetBangKeChiPhi(id, loai, "1"))).ToList();
            return Json(new { data = bangKeChiPhis });
        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = 10000)]
        public async Task<JsonResult> XuLyDuLieu()
        {
            Int32.TryParse(Request.Form["count"], out int count);
            string loaiApDung = Request.Form["LoaiApDung"];
            Decimal.TryParse(Request.Form["TiLeThanhToan"], out decimal tiLeThanhToan);
            Decimal.TryParse(Request.Form["SoTienThanhToan"], out decimal soTienThanhToan);
            List<BangKeChiPhi> data = new List<BangKeChiPhi>();
            string message = String.Empty;
            string title = String.Empty;
            string result = String.Empty;
            try
            {
                List<BangKeChiPhi> bangke = new List<BangKeChiPhi>();
                for (var i = 0; i < count; i++)
                {
                    if(Request.Form["tbID-" + i] != "null") {
                    Decimal.TryParse(Request.Form["tbDonGia-" + i], out decimal dongia);
                    Int32.TryParse(Request.Form["tbThanhTien-" + i], out int thanhtien);
                    Decimal.TryParse(Request.Form["tbBHYTTra-" + i], out decimal bhyttra);
                    Int32.TryParse(Request.Form["tbBNTra-" + i], out int bntra);
                    Decimal.TryParse(Request.Form["tbBNTNThanhToan-" + i], out decimal bhtn);
                    Int32.TryParse(Request.Form["TongSoTien"], out int tongsotien);
                    Decimal.TryParse(Request.Form["TongBHYT"], out decimal tongbhyt);
                    Decimal.TryParse(Request.Form["BNTT"], out decimal tongbhtn);
                    Decimal.TryParse(Request.Form["tbSoLuong-" + i], out decimal soluong);
                    Int32.TryParse(Request.Form["tbSoLuongTT-" + i], out int soluongTT);
                    Int32.TryParse(Request.Form["SoBienLai"], out int sobienlai);
                    Int32.TryParse(Request.Form["tbMaNhomVP-" + i], out int manhomvp);
                    Int32.TryParse(Request.Form["tbSTT-" + i], out int stt);
                    Int32.TryParse(Request.Form["tbCheckBox-" + i], out int Checked);
                    Int32.TryParse(Request.Form["tbMaVP-" + i], out int mavp);
                    Decimal.TryParse(Request.Form["tbMaVaoVien-" + i], out decimal mavaovien);
                    Decimal.TryParse(Request.Form["tbMaQL-" + i], out decimal maql);
                    Int32.TryParse(Request.Form["tbMaDoiTuong-" + i], out int madoituong);
                                     
                    var item = new BangKeChiPhi()
                    {
                        Checked = Checked,
                        id = Request.Form["tbID-" + i],
                        ten = Request.Form["tbNoiDung-" + i],
                        dvt = Request.Form["tbDVT-" + i],
                        dongia = Math.Round(dongia, 2),
                        soluong = Math.Round(soluong, 2),
                        thanhtien = thanhtien,
                        bhyttra = Math.Round(bhyttra, 2),
                        bntra = bntra,
                        bhtn = bhtn,
                        sobienlai = sobienlai,
                        mabn = Request.Form["MaKH"],
                        mavp = mavp,
                        mavaovien = Math.Round(mavaovien, 0),
                        maql = Math.Round(maql, 0),
                        makp = Request.Form["tbMaKP-" + i],
                        nhomvp = Request.Form["tbNhomVP-" + i],
                        manhomvp = manhomvp,
                        hoten = Request.Form["HoTen"],
                        chandoan = Request.Form["ChanDoan"],
                        ngaysinh = Convert.ToDateTime(Request.Form["NgaySinh"], CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                        sothe = Request.Form["SoThe"],
                        ngayvao = Convert.ToDateTime(Request.Form["NgayVao"], CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        gioitinh = Request.Form["GioiTinh"],
                        tenkp = Request.Form["CacKP"],
                        tongsotien = tongsotien,
                        tongbhyt = System.Math.Round(tongbhyt, 2),
                        tongbhtn = System.Math.Round(tongbhtn, 2),
                        SoLuongTT = soluongTT,
                        madoituong = madoituong,
                        STT = stt,
                    };
                    bangke.Add(item);
                    }
                }
                data = (await _bangKeChiPhiSevices.XuLyDuLieu(bangke, tiLeThanhToan, soTienThanhToan, loaiApDung)).ToList();
                if (data.Count > 0 && data != null)
                {
                    message = $"Đã hoàn thành tính toán lại số liệu";
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
            return Json(new { Result = result, Title = title, Message = message, data });
        }
    }
}
