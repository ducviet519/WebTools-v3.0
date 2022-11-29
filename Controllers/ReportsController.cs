using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Extensions;
using WebTools.Models;
using WebTools.Services.Interface;

namespace WebTools.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly IBangKeChiPhiSevices _bangKeChiPhiSevices;
        public ReportsController(IBangKeChiPhiSevices bangKeChiPhiSevices)
        {
            _bangKeChiPhiSevices = bangKeChiPhiSevices;
        }
        public async Task<IActionResult> BangKeVienPhi(string id, string loai)
        {
            ExportReport data = new ExportReport();
            data.BangKeVienPhi = await (_bangKeChiPhiSevices.GetBangKeChiPhi(id, loai, "1"));
            var thongtinnguoibenh = (await (_bangKeChiPhiSevices.GetBangKeChiPhi(id, loai, "1"))).Where(i => i.hoten != null).FirstOrDefault();
            thongtinnguoibenh.ngaysinh = Convert.ToDateTime(thongtinnguoibenh.ngaysinh, CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
            thongtinnguoibenh.ngayvao = Convert.ToDateTime(thongtinnguoibenh.ngayvao, CultureInfo.InvariantCulture).ToString("dd/MM/yyyy hh:mm");
            data.ThongTinNguoiBenh = thongtinnguoibenh;
            data.BarCode = StaticHelper.GenBarCode(data.ThongTinNguoiBenh.mabn);
            data.TenPhieuIn = "BangKeVienPhi";
            return View(data);
        }
    }
}
