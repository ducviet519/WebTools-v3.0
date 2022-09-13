using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models
{
    public class ReportDetail
    {
        public string IDBieuMau { get; set; }
        public string IDPhienBan { get; set; }
        public string TenBM { get; set; }
        public string MaBM { get; set; }
        public string PhanMem { get; set; }
        public string KhoaPhong { get; set; }
        public string URD { get; set; }
        public string ViTriIn { get; set; }
        public string CachIn { get; set; }
        public int STT { get; set; }
        public string PhienBan { get; set; }
        public DateTime NgayBanHanh { get; set; }
        public string TrangThai { get; set; }
        public string GhiChu { get; set; }
        public string FileLink { get; set; }
        public string TrangThaiPM { get; set; }
        public string User { get; set; }

        [EnumDataType(typeof(TrangThai))]
        public TrangThai TrangThaiDropList { get; set; }
    }
    public enum TrangThai
    {
        [Display(Name = "Đang sử dụng")] DangSuDung = 1,
        [Display(Name = "Chưa ban hành")] ChuaBanHanh = 2,
        [Display(Name = "Đã hủy")] DaHuy = 3
    }
}
