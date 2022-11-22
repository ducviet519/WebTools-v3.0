using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using WebTools.Extensions;
using WebTools.Models;
using WebTools.Services.Interface;

namespace WebTools.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IBangKeChiPhiSevices _bangKeChiPhiSevices;
        public ReportsController(IBangKeChiPhiSevices bangKeChiPhiSevices)
        {
            _bangKeChiPhiSevices = bangKeChiPhiSevices;
        }
        public async Task<IActionResult> BangKeVienPhi(string id, string loai)
        {
            //id = "220928193006249049";
            //loai = "2";
            ExportReport data = new ExportReport();
            data.BangKeVienPhi = await (_bangKeChiPhiSevices.GetBangKeChiPhi(id, loai));
            data.ThongTinNguoiBenh = (await (_bangKeChiPhiSevices.GetBangKeChiPhi(id, loai))).Where(i => i.hoten != null).FirstOrDefault();
            data.BarCode = StaticHelper.GenBarCode(data.ThongTinNguoiBenh.mabn);
            data.TenPhieuIn = "BangKeVienPhi";

            //return View(data);
            return new ViewAsPdf("BangKeVienPhi", data)
            {
                //FileName = $"{data.TenPhieuIn}.pdf", //User for click download
                PageSize = Size.A4,
                PageOrientation = Orientation.Portrait,
                IsJavaScriptDisabled = true,
            };
        }
    }
}
