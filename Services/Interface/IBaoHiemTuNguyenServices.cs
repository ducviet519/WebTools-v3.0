using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Services.Interface
{
    public interface IBaoHiemTuNguyenServices
    {
        Task<List<DanhSachKhachHangBHTN>> GetAllKhanhHangThanhToanBHTN();

        Task<List<DanhSachKhachHangBHTN>> SearchKhanhHangThanhToanBHTN(string mabn, string loai = "", string ngayBD = "", string ngayKT = "");

    }
}
