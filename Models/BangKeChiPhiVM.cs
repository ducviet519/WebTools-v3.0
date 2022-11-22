using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using WebTools.Models.Entities;

namespace WebTools.Models
{
    public class BangKeChiPhiVM
    {
        public List<BangKeChiPhi> ListChiPhi { get; set; }
        public BangKeChiPhi BangKeChiPhi { get; set; }
        public IEnumerable<SelectListItem> DonViBaoLanh { get; set; }
        public IEnumerable<SelectListItem> KhoaPhong { get; set; }
        public string id { get; set; }
        public string loai { get; set; }
    }
}
