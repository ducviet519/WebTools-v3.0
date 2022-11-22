using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Models
{
    public class ExportReport
    {
        public string TenPhieuIn { get; set; }
        public string BarCode { get; set; }

        public List<BangKeChiPhi> BangKeVienPhi { get; set; }
        public BangKeChiPhi ThongTinNguoiBenh { get; set; }
    }
}
