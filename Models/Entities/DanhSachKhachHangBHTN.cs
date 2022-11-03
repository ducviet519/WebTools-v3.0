using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models.Entities
{
    public class DanhSachKhachHangBHTN
    {
        public string id { get; set; }
        public string mabn { get; set; }
        public string hoten { get; set; }
        public string ngayvao { get; set; }
        public string ngaythutien { get; set; }
        public string sobienlai { get; set; }
        public int loainv { get; set; }
    }
}
