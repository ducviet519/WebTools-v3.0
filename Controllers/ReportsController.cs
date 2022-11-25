using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            data.ThongTinNguoiBenh = (await (_bangKeChiPhiSevices.GetBangKeChiPhi(id, loai, "1"))).Where(i => i.hoten != null).FirstOrDefault();
            data.BarCode = StaticHelper.GenBarCode(data.ThongTinNguoiBenh.mabn);
            data.TenPhieuIn = "BangKeVienPhi";
            return View(data);
        }
    }
}
